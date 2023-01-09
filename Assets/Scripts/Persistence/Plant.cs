using System;
using Coherence;
using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Plant : MonoBehaviour
{
    public InputActionReference plantAction;
    public Animator animator;
    public CoherenceSync sync;
    
    public Transform plantPosition;
    public Flower plantPrefab;
    public Transform flowerPreview;
    public Transform bulbPreview;
    public LayerMask groundLayer;

    [Header("Flower preview colours")]
    public Color regular = new Color(.37f, .68f, 0f, .29f);
    public Color blocked = new Color(.67f, .18f, 0f, .29f);

    private FlowersHandler _flowersHandler;
    private Flower _newPlant;
    private Material _previewMaterial;
    private Collider[] _collisionResults = new Collider[1];
    private RaycastHit[] _raycastResults = new RaycastHit[1];
    private bool _isPreviewingPlacement;
    private int _collidersFound;
    private int _raycastPointsFound;
    private bool _hasCoherenceSync;

    private void Awake()
    {
        _hasCoherenceSync = sync != null;
    }

    private void Start()
    {
        _flowersHandler = FindObjectOfType<FlowersHandler>();
        // Prepare preview
        flowerPreview.gameObject.SetActive(false);
        bulbPreview.gameObject.SetActive(false);
        _previewMaterial = flowerPreview.GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
        plantAction.asset.Enable();
        plantAction.action.started += OnPlantButtonPressed;
        plantAction.action.canceled += OnPlantButtonReleased;
    }

    private void OnDisable()
    {
        plantAction.action.started -= OnPlantButtonPressed;
        plantAction.action.canceled -= OnPlantButtonReleased;
    }

    private void OnPlantButtonPressed(InputAction.CallbackContext obj)
    {
        flowerPreview.gameObject.SetActive(true);
        bulbPreview.gameObject.SetActive(true);
        _isPreviewingPlacement = true;
        animator.SetBool("CarryingSmall", true);
    }

    private void Update()
    {
        if (_isPreviewingPlacement)
        {
            // Is there even space?
            _collidersFound = Physics.OverlapSphereNonAlloc(plantPosition.position + Vector3.up * 1f, .4f,
                _collisionResults, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            if (_collidersFound > 0)
            {
                // No space
                _previewMaterial.color = blocked;
                flowerPreview.transform.localPosition = Vector3.zero;
            }
            else
            {
                // There's space. Where would the plant go? (only query Ground layer)
                _raycastPointsFound = Physics.RaycastNonAlloc(plantPosition.position + Vector3.up, Vector3.down,
                    _raycastResults, 1.1f, groundLayer, QueryTriggerInteraction.Ignore);

                if (_raycastPointsFound >= 1)
                {
                    // Found ground to plant on
                    _previewMaterial.color = regular;
                    flowerPreview.transform.position = _raycastResults[0].point;
                }
                else
                {
                    // Didn't find any ground. Maybe the player is currently in the air.
                    _previewMaterial.color = blocked;
                    flowerPreview.transform.localPosition = Vector3.zero;
                }
            }
        }
    }

    private void OnPlantButtonReleased(InputAction.CallbackContext obj)
    {
        flowerPreview.gameObject.SetActive(false);
        bulbPreview.gameObject.SetActive(false);
        _isPreviewingPlacement = false;
        animator.SetBool("CarryingSmall", false);

        if (_collidersFound == 0 && _raycastPointsFound == 1)
        {
            _flowersHandler.AddFlower(); // Will make space for a new flower, if needed
            
            animator.SetTrigger("Plant");
            if (_hasCoherenceSync) sync.SendCommand<Animator>(nameof(Animator.SetTrigger), MessageTarget.Other, "Plant");
            
            Vector3 facingDirection = Random.insideUnitSphere;
            facingDirection.y = 0f;
            
            _newPlant = Instantiate<Flower>(plantPrefab, flowerPreview.position, Quaternion.LookRotation(facingDirection, Vector3.up));
            _newPlant.gameObject.name = "Flower";
            _newPlant.Plant();
        }
    }
}
