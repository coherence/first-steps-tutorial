using Coherence;
using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles switching authority of the robot arm and the crate, whether this is currently held by the arm or not.
/// The <see cref="RequestAuthority"/> method is invoked directly from the UI button in the scene.
/// </summary>
public class RobotArmAuthority : MonoBehaviour
{
    public CoherenceSync robotArm;
    public CoherenceSync crate;

    [Header("UI")]
    public GameObject authorityMessage;
    public GameObject remoteMessage;
    public Button requestAuthorityButton;

    private bool _hasArmAuthority;
    private bool _hasCrateAuthority;

    private void OnEnable()
    {
        crate.OnStateAuthority.AddListener(OnCrateAuthority);
        crate.OnStateRemote.AddListener(OnCrateRemote);
    }

    private void OnDisable()
    {
        crate.OnStateAuthority.RemoveListener(OnCrateAuthority);
        crate.OnStateRemote.RemoveListener(OnCrateRemote);
    }

    /// <summary>
    /// Invoked by the UI button in the scene.
    /// </summary>
    public void RequestAuthority()
    {
        robotArm.RequestAuthority(AuthorityType.Full);
        crate.RequestAuthority(AuthorityType.Full);

        requestAuthorityButton.interactable = false;
    }

    private void OnCrateAuthority()
    {
        requestAuthorityButton.interactable = false;
        authorityMessage.SetActive(true);
        remoteMessage.SetActive(false);
        crate.GetComponent<Rigidbody>().isKinematic = crate.transform.parent != null;
    }

    private void OnCrateRemote()
    {
        requestAuthorityButton.interactable = true;
        authorityMessage.SetActive(false);
        remoteMessage.SetActive(true);
        crate.GetComponent<Rigidbody>().isKinematic = true;
    }
}
