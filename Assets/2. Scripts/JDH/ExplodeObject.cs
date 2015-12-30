using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class explosionInfo : MonoBehaviour
{
	public Vector3 force;
	public float LifeTime;
	public float FadeTime;
	public bool UseGravity = false;
	bool explodeProcessing = false;

	void Start()
	{
		if( LifeTime < FadeTime )
		{
			LifeTime = FadeTime;
		}
	}

	void Update( )
	{
		if( gameObject.GetComponent<Rigidbody>() == null )
		{
			if( UseGravity )
			{
				transform.position += force * Time.deltaTime;
				force += ( Physics.gravity * Time.deltaTime );
			}
			else
				transform.position += force * Time.deltaTime;
		}

		if( explodeProcessing && ( ( LifeTime > 0.0f ) || ( FadeTime > 0.0f ) ) )
		{
			LifeTime -= Time.deltaTime;

			if( LifeTime <= 0.0f )
			{
				Object.Destroy( transform.parent.gameObject );
			}
			else if( LifeTime <= FadeTime )
			{
				Material tempMaterial = GetComponent<Renderer>().material;
				if( ( tempMaterial != null ) && ( tempMaterial.mainTexture != null ) ) 
				{
					Color c = tempMaterial.color;
					if( c != null )
					{
						c.a = 1.0f - ( ( FadeTime - LifeTime ) / FadeTime );
						GetComponent<Renderer>().material.color = c;
					}
				}

			}
		}
	}

	public void PlayExplode()
	{
		explodeProcessing = true;

		if( FadeTime > 0.0f )
			GetComponent<Renderer>().material.shader = Shader.Find( "Transparent/Diffuse" ); // Change Transparent Shader for Fade

		if( GetComponent<Rigidbody>() != null )
			GetComponent<Rigidbody>().AddForce( force, ForceMode.Impulse );
	}
};

public class ExplodeObject : MonoBehaviour
{
	public bool UseGravity = true;
	public bool UseRigidBody = true;
	public bool UseCollider = true;
	public int SkipTriangle = 0;
	public float SkipLessThanArea = -1.0f;

	public string ForceCheckObjectName = "";
	public string ForceCheckTagName = "";
	public LayerMask ForceCheckLayer = -1;

	public float ParticleExplosionForce = 1;
	public float ParticleThick = 0.15f;

	public float LifeTime = 5;
	public float FadeTime = 0.5f;
	public float LimitForce = 1.0f;
	public float ExplosionForce = -1.0f;
	public float ExplosionRadius = -1.0f;
    public int check = 0;

	MeshFilter meshFileter;
	Mesh mesh;
	List<GameObject> particleList = new List<GameObject>();
	GameObject explosionParticleGroup;

	// Use this for initialization
	void Start( )
	{
		explosionParticleGroup = new GameObject();
		explosionParticleGroup.transform.parent = transform.parent ? transform.parent : null;
		explosionParticleGroup.transform.localPosition = transform.localPosition;
		explosionParticleGroup.transform.localRotation = transform.localRotation;
		explosionParticleGroup.transform.localScale = transform.localScale;
		explosionParticleGroup.name = "ExplosionGroup_" + name;
		meshFileter = GetComponent<MeshFilter>();
		
		// Make New Meshs
		mesh = meshFileter.mesh;
		if( mesh != null )
		{
			for( int TrianglesIndex = 0; TrianglesIndex < mesh.triangles.Length; TrianglesIndex += ( 3 * ( SkipTriangle + 1 ) ) )
			{

				if( SkipLessThanArea > 0.0f )
				{
					Vector3 v = Vector3.Cross( mesh.vertices[mesh.triangles[TrianglesIndex + 0]] -
												mesh.vertices[mesh.triangles[TrianglesIndex + 1]],
												mesh.vertices[mesh.triangles[TrianglesIndex + 0]] -
												mesh.vertices[mesh.triangles[TrianglesIndex + 2]] );

					if( ( v.magnitude * 0.5f ) < SkipLessThanArea )
					{
						continue;
					}
				}

				GameObject go = GameObject.CreatePrimitive( PrimitiveType.Quad );
				Mesh newMesh = new Mesh();
				
				


				// make 3d Plane
				newMesh.vertices = new Vector3[]
				{
					mesh.vertices[mesh.triangles[TrianglesIndex+0]],
					mesh.vertices[mesh.triangles[TrianglesIndex+1]],
					mesh.vertices[mesh.triangles[TrianglesIndex+2]],
					mesh.vertices[mesh.triangles[TrianglesIndex+0]] - mesh.normals[mesh.triangles[TrianglesIndex+0]] * ParticleThick,
					mesh.vertices[mesh.triangles[TrianglesIndex+1]] - mesh.normals[mesh.triangles[TrianglesIndex+1]] * ParticleThick,
					mesh.vertices[mesh.triangles[TrianglesIndex+2]] - mesh.normals[mesh.triangles[TrianglesIndex+2]] * ParticleThick
				};

				newMesh.uv = new Vector2[]
				{
					mesh.uv[mesh.triangles[TrianglesIndex+0]],
					mesh.uv[mesh.triangles[TrianglesIndex+1]],
					mesh.uv[mesh.triangles[TrianglesIndex+2]],
					mesh.uv[mesh.triangles[TrianglesIndex+0]],
					mesh.uv[mesh.triangles[TrianglesIndex+1]],
					mesh.uv[mesh.triangles[TrianglesIndex+2]]
				};

				newMesh.triangles = new int[]
				{
					0, 2, 3,
					2, 5, 3,
					0, 3, 1,
					1, 3, 4,
					1, 4, 2,
					2, 4, 5,
					2, 0, 1,
					5, 4, 3
				};

				newMesh.RecalculateNormals();
				
				// Recalculate Tangent
				TangentSolver( newMesh );
				
				// For AddForce Direction
				Vector3 dirVector = GetTriangleCentroid( newMesh.vertices[0], newMesh.vertices[1], newMesh.vertices[2] ) - mesh.bounds.center;
				dirVector.Normalize();
				
				go.transform.parent = explosionParticleGroup.transform;
				go.transform.localPosition = Vector3.zero;
				go.transform.localRotation = Quaternion.identity;
				go.transform.localScale = Vector3.one;

				go.GetComponent<MeshFilter>().sharedMesh = newMesh;

				if( UseCollider ){
					go.GetComponent<MeshCollider>().sharedMesh = newMesh;
					go.GetComponent<MeshCollider>().convex = true;
				}
				else
					go.GetComponent<MeshCollider>().enabled = false;			

				go.GetComponent<Renderer>().sharedMaterials = GetComponent<Renderer>().sharedMaterials;
				
				go.AddComponent<explosionInfo>().force = dirVector * ParticleExplosionForce;
				go.GetComponent<explosionInfo>().LifeTime = LifeTime;
				go.GetComponent<explosionInfo>().FadeTime = FadeTime;

				if( UseRigidBody )
					go.AddComponent<Rigidbody>().useGravity = UseGravity;
				else
					go.GetComponent<explosionInfo>().UseGravity = UseGravity;

				particleList.Add( go );
				
			}			
			explosionParticleGroup.SetActive(false);
		}

	}

	// Update is called once per frame
	void Update( )
	{

	}

	// Player Explode
	public void Explode()
	{
		gameObject.SetActive( false );
		explosionParticleGroup.SetActive( true );
		explosionParticleGroup.transform.position = transform.position;
		explosionParticleGroup.transform.localPosition = transform.localPosition;
		explosionParticleGroup.transform.localRotation = transform.localRotation;
		explosionParticleGroup.transform.localScale = transform.localScale;
		for( int i = 0; i < particleList.Count; i++ )
		{
			particleList[i].GetComponent<explosionInfo>().PlayExplode();
		}
		Object.Destroy( gameObject );
	}

    void OnCollisionEnter(Collision other)
	{
        if (check == 1)
            return;
		if( ( ( other.gameObject.tag == ForceCheckTagName ) || ( ForceCheckTagName == "" ) ) &&
			( ( other.gameObject.name == ForceCheckObjectName ) || ( ForceCheckObjectName == "" ) ) &&
			( ( ( 1 << other.gameObject.layer ) & ForceCheckLayer.value ) != 0 )
			)
		{
			float force = other.relativeVelocity.magnitude;
			float mass = 1.0f;
			if( other.rigidbody ) { 
				mass = other.rigidbody.mass;
				if( ( ExplosionForce > 0.0f ) || ( ExplosionRadius > 0.0f ) )
					other.rigidbody.AddExplosionForce( ExplosionForce, gameObject.transform.position, ExplosionRadius );
			}

			ExplodeObject ep = gameObject.GetComponent<ExplodeObject>( );
			if( ( ep != null ) && ( ( force * mass ) > LimitForce ) )
			{
				ep.Explode( );
			}
        }
    }
    public void StartEffect()
    {


        ExplodeObject ep = gameObject.GetComponent<ExplodeObject>();
        if ((ep != null) && ((5 * 1.0f) > LimitForce))
        {
            ep.Explode();
        }
    }
    void OnCollisionExit(Collision other)
    {

    }

    private static Vector3 GetTriangleCentroid(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3 result = new Vector3();

        result = (v1 + v2 + v3) / 3.0f;

        return result;
    }

	public static void TangentSolver(Mesh mesh)
    {
        int triangleCount = mesh.triangles.Length / 3;
        int vertexCount = mesh.vertices.Length;
 
        Vector3[] tan1 = new Vector3[vertexCount];
        Vector3[] tan2 = new Vector3[vertexCount];
        Vector4[] tangents = new Vector4[vertexCount];
        int a = 0;
        while(a < triangleCount)
        {
            long i1 = mesh.triangles[a++];
            long i2 = mesh.triangles[a++];
            long i3 = mesh.triangles[a++];
 
            Vector3 v1 = mesh.vertices[i1];
            Vector3 v2 = mesh.vertices[i2];
            Vector3 v3 = mesh.vertices[i3];
 
            Vector2 w1 = mesh.uv[i1];
            Vector2 w2 = mesh.uv[i2];
            Vector2 w3 = mesh.uv[i3];
 
            float x1 = v2.x - v1.x;
            float x2 = v3.x - v1.x;
            float y1 = v2.y - v1.y;
            float y2 = v3.y - v1.y;
            float z1 = v2.z - v1.z;
            float z2 = v3.z - v1.z;
 
            float s1 = w2.x - w1.x;
            float s2 = w3.x - w1.x;
            float t1 = w2.y - w1.y;
            float t2 = w3.y - w1.y;
 
            float r = 1.0f / (s1 * t2 - s2 * t1);
 
            Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
            Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
 
            tan1[i1] += sdir;
            tan1[i2] += sdir;
            tan1[i3] += sdir;
 
            tan2[i1] += tdir;
            tan2[i2] += tdir;
            tan2[i3] += tdir;
        }
        for (a = 0; a < vertexCount; a++)
        {
            Vector3 n = mesh.normals[a];
            Vector3 t = tan1[a];
            tangents[a] = (t - n * Vector3.Dot(n, t)).normalized;
            tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
        }
        mesh.tangents = tangents;
    }
}