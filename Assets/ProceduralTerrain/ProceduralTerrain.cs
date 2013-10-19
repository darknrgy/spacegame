using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using UnityEditor;
using Random = System.Random;
using System.Security.Cryptography;
using System.Collections.Generic;


public class ProceduralTerrain : MonoBehaviour {
	
	public float startingAmplitude = 100;
	public float amplitudeDelta = 2;
	public float iterations = 4;
	
	protected Hashtable hashtable;
	protected Int32 index = 0;
	Vector3[] newVertices;
	Vector2[] newUVs;
	
	
	void Start () {
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		
		float amplitude = startingAmplitude;
		
		for (int i = 0; i < iterations; i ++){
			increaseResolution(mesh, amplitude);
			amplitude /= amplitudeDelta;
		}
		mesh.Optimize();
		AssetDatabase.CreateAsset(mesh, "Assets/ProceduralTerrain/Planet01.asset");
		
	}
	
	void increaseResolution(Mesh mesh, float amplitude){
		
		
		Vector3[] vertices = mesh.vertices;
		
		Int32 newCount = vertices.Length * 4;
		newVertices = new Vector3[newCount];
		newUVs = new Vector2[newCount];
		hashtable = new Hashtable();
		index = 0;
				
		
		for (Int32 i = 0; i < vertices.Length - 2; i += 3){
			
			Vector3 i0 = vertices[i];
			Vector3 i1 = vertices[i+1];
			Vector3 i2 = vertices[i+2];
			
			Vector3 m01 = getAdjustedMidpoint(i0, i1);
			Vector3 m12 = getAdjustedMidpoint(i1, i2);
			Vector3 m02 = getAdjustedMidpoint(i2, i0);
			
			
			
			addAndHashVector(i0);
			addAndHashVector(m01);
			addAndHashVector(m02);
			
			addAndHashVector(i1);
			addAndHashVector(m12);
			addAndHashVector(m01);
			
			addAndHashVector(i2);
			addAndHashVector(m02);
			addAndHashVector(m12);
			
			addAndHashVector(m02);
			addAndHashVector(m01);
			addAndHashVector(m12);
			
		}
		
		adjustByRealPoint(amplitude);
		
		
		
		mesh.Clear();
		mesh.vertices = newVertices;
		Int32 count = Enumerable.Range(0, newCount).ToArray().Length;
		Debug.Log("count of triangles" + newCount + "real count is " + newVertices.Length);
		mesh.triangles = Enumerable.Range(0, newCount).ToArray();
		mesh.uv = newUVs;
		mesh.RecalculateNormals();
		
	}
	
	void adjustByRealPoint(float amplitude){
		
		Random rand = new Random();
		
		foreach (DictionaryEntry pair in hashtable){
			
			// correct for normal and add random height
			List<Int32> points = (List<Int32>) pair.Value;
			Vector3 vector;
			Int32 firstPoint = points.ElementAt(0);
			vector = newVertices[firstPoint];
			vector += vector.normalized * amplitude * (float) (rand.NextDouble() - 0.5);					
			
			// calculate Lat/Lon for texture mapping
			
			
			Vector2 xz = new Vector2(vector.x, vector.z);
			
			float longitude = Vector2.Angle(new Vector2(1,0), xz);
						
			if (vector.z > 0) longitude = 360 - longitude;
			
			float lattitude = Vector3.Angle(new Vector3(0,1,0), vector);
			
			longitude /= 361;
			lattitude /= 180;
			
			
			longitude = Math.Abs(longitude - (float) Math.Floor((double) longitude));
			lattitude = Math.Abs(lattitude - (float) Math.Floor((double) lattitude));
			
			
			foreach (Int32 point in points){
				newVertices[point] = vector;
				newUVs[point] = new Vector2(longitude, lattitude);
				
			}
		}		
	}
	
	Vector3 normalize(Vector3 vector){
		return vector.normalized * 100; 
	}
	
	void normalizeHeight(){
		for (Int32 i = 0; i < newVertices.Length; i++){
			newVertices[i] = normalize(newVertices[i]);
		}
	}
	
	void addAndHashVector(Vector3 vector){
		MD5 md5 = MD5.Create();
		List<Int32> points;
		newVertices[index] = clone (vector);
		string hash = getString(md5.ComputeHash(getBytes(vector.ToString())));
		if (!hashtable.ContainsKey(hash)){
			points = new List<Int32>();
			hashtable.Add(hash, points);
		}
		points = (List<Int32>) hashtable[hash];
		points.Add(index);
		
		
		
		index += 1;
	}
	
	static byte[] getBytes(string str)
	{
	    byte[] bytes = new byte[str.Length * sizeof(char)];
	    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
	    return bytes;
	}
	
	static string getString(byte[] bytes)
	{
	    char[] chars = new char[bytes.Length / sizeof(char)];
	    System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
	    return new string(chars);
	}
				
	Vector3 clone(Vector3 v){
		return new Vector3(v.x, v.y, v.z);
	}
	
	Vector3 getMidpoint(Vector3 a, Vector3 b){
		return new Vector3((a.x + b.x) / 2, (a.y + b.y) / 2, (a.z + b.z) / 2);
	}
	
	Vector3 getAdjustedMidpoint(Vector3 a, Vector3 b){
		
		Vector3 normalAverage = getMidpoint(normalize(a), normalize(b));
		Vector3 normal = normalize(normalAverage);
		Vector3 offset = normal - normalAverage;
		
		Vector3 realVector = getMidpoint(a,b) + offset;
		return realVector;
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
	}
	
		void recenterGeometry(){
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		
		Vector3 v = clone (vertices[0]);
		float low_x = v.x;
		float high_x = v.x;
		
		float low_y = v.y;
		float high_y = v.y;
		
		float low_z = v.z;
		float high_z = v.z;
		
		
		foreach (Vector3 vector in vertices){
			if (vector.x <  low_x) low_x = vector.x;
			if (vector.x > high_x) high_x = vector.x;
			
			if (vector.y <  low_y) low_y = vector.y;
			if (vector.y > high_y) high_y = vector.y;
			
			if (vector.z <  low_z) low_z = vector.z;
			if (vector.z > high_z) high_z = vector.z;
		}
		
		float center_x = (low_x + high_x) / 2;
		float center_y = (low_y + high_y) / 2;
		float center_z = (low_z + high_z) / 2;
		
		Debug.Log("x: " + center_x + ", y: " + center_y + ", z: " + center_z);
		
		for (var i = 0; i < vertices.Length; i++){
			
			vertices[i].x -= center_x;
			vertices[i].y -= center_y;
			vertices[i].z -= center_z;
		}
		
		AssetDatabase.CreateAsset(mesh, "Assets/PerfectIcosahedron.asset");
		
		
	}
}
