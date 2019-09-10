using UnityEngine;

public class ScrGameManager : MonoBehaviour {

    #region Singlton

    private static ScrGameManager m_instance;
    public static ScrGameManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = GameObject.FindObjectOfType<ScrGameManager>();

            return m_instance;
        }
    }

    #endregion

    [SerializeField]private Transform spawnPoint;

    public void Respawn(Transform t)
    {
        t.position = spawnPoint.position;
        t.rotation = spawnPoint.rotation;
    }
}
