using Coherence;
using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.UI;

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
