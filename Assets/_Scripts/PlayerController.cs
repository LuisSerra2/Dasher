using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IGameStateController
{
    public float speed;
    public LayerMask ground;

    private Vector3 targetPosition;
    private bool isMoving = false;

    [Space(20)]

    [Header("Abilities")]

    public GameObject bulletPrefab;
    public float bulletCooldown;

    [Space]

    public GameObject nukePrefab;
    public float nukeCooldown;

    [Space]

    public GameObject hurricanePrefab;
    public float hurricaneCooldownDivideByTwo;

    private Dictionary<string, int> abilityUses = new Dictionary<string, int>();
    private Dictionary<string, float> abilityCooldowns = new Dictionary<string, float>();
    private Dictionary<string, float> abilityCooldownTimer = new Dictionary<string, float>();

    private void Start()
    {
        abilityUses["BulletAbility"] = 0;
        abilityUses["NukeAbility"] = 0;
        abilityUses["HurricaneAbility"] = 0;

        abilityCooldownTimer["BulletAbility"] = bulletCooldown; 
        abilityCooldownTimer["NukeAbility"] = nukeCooldown; 
        abilityCooldownTimer["HurricaneAbility"] = hurricaneCooldownDivideByTwo / 2; 

        abilityCooldowns["BulletAbility"] = 0;
        abilityCooldowns["NukeAbility"] = 0;
        abilityCooldowns["HurricaneAbility"] = 0;

        UIManager.Instance.UpdateAbilitiesIndexText(abilityUses);
    }

    public void Idle()
    {

    }

    public void Playing()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            MovementWithRaycast();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseAbility("BulletAbility");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            UseAbility("NukeAbility");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseAbility("HurricaneAbility");
        }

        if (isMoving)
        {
            MovePlayer();
        }

        UpdateCooldowns();
    }

    public void Dead()
    {

    }


    private void MovementWithRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, ground))
        {
            if (hitInfo.collider != null)
            {
                targetPosition = hitInfo.point;
                isMoving = true;
            }
        }
    }

    private void MovePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }

    private void AbilityRaycastPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, ground))
        {
            if (hitInfo.collider != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 1), bulletPrefab.transform.rotation);
                bullet.transform.LookAt(hitInfo.point);
                bullet.transform.rotation = Quaternion.Euler(90, bullet.transform.rotation.eulerAngles.y + 230, bullet.transform.rotation.eulerAngles.z);

                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                if (bulletComponent != null)
                {
                    bulletComponent.SetDirection((hitInfo.point - transform.position).normalized);
                }
            }
        }
    }

    public void IncrementAbilityUse(string abilityName)
    {
        if (abilityUses.ContainsKey(abilityName))
        {
            abilityUses[abilityName]++;
        } else
        {
            abilityUses[abilityName] = 1;
        }
        UIManager.Instance.UpdateAbilitiesIndexText(abilityUses);
    }

    private void UseAbility(string abilityName)
    {
        if (abilityUses.ContainsKey(abilityName) && abilityUses[abilityName] > 0 && abilityCooldowns[abilityName] <= 0)
        {
            if (abilityName == "BulletAbility")
            {
                AbilityRaycastPosition();
                abilityUses[abilityName]--;
            }

            if (abilityName == "NukeAbility")
            {
                Instantiate(nukePrefab, new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), nukePrefab.transform.rotation);
                abilityUses[abilityName]--;
            }
            
            if (abilityName == "HurricaneAbility")
            {
                Instantiate(hurricanePrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), hurricanePrefab.transform.rotation, transform);
                abilityUses[abilityName]--;
            }

            abilityCooldowns[abilityName] = abilityCooldownTimer[abilityName];
            UIManager.Instance.UpdateAbilitiesIndexText(abilityUses);
        }
    }

    private void UpdateCooldowns()
    {
        List<string> keys = new List<string>(abilityCooldowns.Keys);
        foreach (string key in keys)
        {
            if (abilityCooldowns[key] > 0)
            {
                abilityCooldowns[key] -= Time.deltaTime;
            }
        }
        UIManager.Instance.UpdateAbilitiesIndexText(abilityUses);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.parent != null && collision.collider.transform.parent.GetComponent<EnemyController>() != null)
        {
            GameController.Instance.ChangeState(GameManager.Dead);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(out Ability ability))
        {
            ability.ApplyEffect(this);
        }
    }
}
