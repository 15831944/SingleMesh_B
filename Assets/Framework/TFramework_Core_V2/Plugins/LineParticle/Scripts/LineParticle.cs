using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ParticleSystem))]
public class LineParticle : MonoBehaviour
{

	#region Properties

	[SerializeField]
	[Range (1, 1000)]
	private int m_Resolution = 10;
	[SerializeField]
	private float m_Size = 0.25f;
	[SerializeField]
	private Gradient m_ColorLine;
	[SerializeField]
	private FunctionDrawOption m_DrawOption;
	[SerializeField]
	private FunctionColorOption m_ColorOption;
	[SerializeField]
	private Vector3[] m_Points;

	private int m_CurrentPointLength = 0;
	private int m_CurrentResolution = 0;
	private ParticleSystem m_ParticleSystem;
	private ParticleSystemRenderer m_ParticleSystemRenderer;
	private ParticleSystem.Particle[] m_ParticlePoints;
	private static Transform m_Transform;

	private enum FunctionDrawOption
	{
		Linear,
		Sine
	}

	private enum FunctionColorOption
	{
		Static,
		Dynamic
	}

	private delegate Vector3 FunctionDrawDelegate (Vector3 point,Vector3 direction,int index);

	private delegate float FunctionColorDelegate (int index,int length);

	private static FunctionDrawDelegate[] m_FunctionDrawDelegates = {
		Linear,
		Sine
	};

	private static FunctionColorDelegate[] m_FunctionColorDelegates = {
		Static,
		Dynamic
	};

	#endregion

	#region MonoBehaviour

	private void OnEnable ()
	{
		if (m_ParticleSystem != null) {
			DrawPoint ();
		}
	}

	private void OnDisable ()
	{
		if (m_ParticleSystem != null) {
			m_ParticleSystem.Stop ();
		}
	}

	private void Awake ()
	{
		m_Transform = this.GetComponent<Transform> ();
		m_ParticleSystem = this.GetComponent<ParticleSystem> ();
		m_ParticleSystemRenderer = this.GetComponent<ParticleSystemRenderer> ();
		m_ParticleSystem.loop = false;
		if (m_ParticleSystem.duration > 0.1f) {
			Debug.LogError ("Particle System duration may not work, DURATION must set zero.");
		}
		m_ParticleSystem.scalingMode = ParticleSystemScalingMode.Shape;
		m_ParticleSystem.playOnAwake = false;
		m_Transform.rotation = Quaternion.identity;
		CreatePoints ();
	}

	private void Update ()
	{
		if (m_CurrentResolution != m_Resolution || m_ParticlePoints == null || m_CurrentPointLength != m_Points.Length) {
			CreatePoints ();
		}
		m_ParticleSystem.SetParticles (m_ParticlePoints, m_ParticlePoints.Length);
		DrawPoint ();
	}

    #endregion

    #region Methods

    public void SetPoints(Vector3[] points) {
        m_Points = points;
        //DrawPoint();
    }

	public void CreatePoints ()
	{
		if (m_Points.Length == 0) {
			m_Points = new Vector3[]{ Vector3.zero };
		}
		m_CurrentPointLength = m_Points.Length;
		m_CurrentResolution = m_Resolution;
		m_ParticlePoints = new ParticleSystem.Particle[m_Resolution * (m_Points.Length - 1)];
		DrawPoint ();
	}

	public void DrawPoint ()
	{
		if (m_Points.Length <= 1)
			return;
		var segment = m_Resolution / (m_Points.Length - 1);
		for (int i = 0, j = 1; i < m_Points.Length; i++, j = j + 1 > m_Points.Length - 1 ? j = 0 : j + 1) {
			var point1 = m_Points [i];
			var point2 = m_Points [j];
			var direction = (point2 - point1) / m_Resolution;
			var funcDraw = m_FunctionDrawDelegates [(int)m_DrawOption];
			var funcColor = m_FunctionColorDelegates [(int)m_ColorOption];
			for (int x = m_Resolution * i, y = 0; x < m_Resolution * j; x++, y++) {
				m_ParticlePoints [x].position = funcDraw (point1, direction, y);
				m_ParticlePoints [x].startColor = m_ColorLine.Evaluate (funcColor (x, m_ParticlePoints.Length));
				m_ParticlePoints [x].startSize = m_Size;
			}
		}
		m_ParticleSystem.SetParticles (m_ParticlePoints, m_ParticlePoints.Length);
	}

	private static Vector3 Linear (Vector3 point, Vector3 direction, int index)
	{
		return point + (direction * index) - m_Transform.position;
	}

	private static Vector3 Sine (Vector3 point, Vector3 direction, int index)
	{
		var result = point + (direction * index) - m_Transform.position;
		result.y += 0.5f + 0.5f * Mathf.Sin (0.5f * Mathf.PI * result.y);
		return result;
	}

	private static float Static (int index, int length)
	{
		return (float)index / length;
	}

	private static float Dynamic (int index, int length)
	{
		return (((float)index / length) + Time.time) % 1f;
	}

	#endregion

	#region Getter && Setter

	public void SetPosition (int index, Vector3 position)
	{
		if (index > m_Points.Length - 1 || index < 0 || m_Points == null)
			return;
		m_Points [index] = position;
		DrawPoint ();
	}

	public Vector3 GetPosition (int index)
	{
		if (index > m_Points.Length - 1 || index < 0 || m_Points == null)
			return Vector3.zero;
		return m_Points [index];
	}

	public void SetActive (bool value)
	{
		if (value == false) {
			m_ParticleSystem.Clear ();
		} else {
			DrawPoint ();
		}
		gameObject.SetActive (value);
	}

	#endregion

}