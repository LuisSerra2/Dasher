using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>, IGameStateController
{
    public float speed;
    public LayerMask ground;

    public GameObject playerSplit;

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

    [Space(20)]

    [Header("Ability Sprites")]
    public Sprite defaultBulletSprite;
    public Sprite disabledBulletSprite;
    public Image bulletImage;

    public Sprite defaultNukeSprite;
    public Sprite disabledNukeSprite;
    public Image nukeImage;

    public Sprite defaultHurricaneSprite;
    public Sprite disabledHurricaneSprite;
    public Image hurricaneImage;

    [Space(20)]

    [Header("Death")]
    public Material playerMaterial;
    public float deathTime = 0f;
    public float maxDeathTime = 1f;

    private Dictionary<string, int> abilityUses = new Dictionary<string, int>();
    private Dictionary<string, float> abilityCooldowns = new Dictionary<string, float>();
    private Dictionary<string, float> abilityCooldownTimer = new Dictionary<string, float>();

    private InputsManager inputsManager;

    private void Start()
    {
        playerMaterial.SetFloat("_DeathTimer", 0);

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
        UpdateAbilitySprites();

        inputsManager = InputsManager.Instance;
        UIManager.Instance.UpdateInputsText(inputsManager.customInput.keyQ, inputsManager.customInput.keyW, inputsManager.customInput.keyE);
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
        if (Input.GetKeyDown(inputsManager.customInput.keyQ) && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            UseAbility("BulletAbility");
        }

        if (Input.GetKeyDown(inputsManager.customInput.keyW) && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            UseAbility("NukeAbility");
        }
        if (Input.GetKeyDown(inputsManager.customInput.keyE) && !Input.GetKeyDown(KeyCode.LeftShift))
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
        WaveManager.Instance.RemoveEnemies();
        OnPlayerDeath();
    }

    public void OnPlayerDeath()
    {
        if (WaveManager.Instance.PlayerColorChange())
        {
            deathTime += Time.deltaTime;
            deathTime = Mathf.Clamp(deathTime, 0f, maxDeathTime);

            playerMaterial.SetFloat("_DeathTimer", deathTime / maxDeathTime);
        }        
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
                    //bulletComponent.ApplyQAbilityProperties(FindObjectOfType<QAbility>());
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
        UpdateAbilitySprites();
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
                GameObject nukePrefabClone = Instantiate(nukePrefab, new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), nukePrefab.transform.rotation);

                //Nuke nuke = nukePrefabClone.GetComponent<Nuke>();
                //if (nuke != null)
                //{
                //    nuke.ApplyWAbilityProperties(FindObjectOfType<WAbility>());
                //}

                abilityUses[abilityName]--;
            }

            if (abilityName == "HurricaneAbility")
            {
                Instantiate(hurricanePrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), hurricanePrefab.transform.rotation, transform);
                abilityUses[abilityName]--;
            }

            abilityCooldowns[abilityName] = abilityCooldownTimer[abilityName];
            UIManager.Instance.UpdateAbilitiesIndexText(abilityUses);
            UpdateAbilitySprites();
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
        UpdateAbilitySprites();
    }

    private void UpdateAbilitySprites()
    {
        bulletImage.sprite = abilityUses["BulletAbility"] > 0 ? defaultBulletSprite : disabledBulletSprite;
        nukeImage.sprite = abilityUses["NukeAbility"] > 0 ? defaultNukeSprite : disabledNukeSprite;
        hurricaneImage.sprite = abilityUses["HurricaneAbility"] > 0 ? defaultHurricaneSprite : disabledHurricaneSprite;
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
        if (other.transform.root.TryGetComponent(out AbilityEffect ability))
        {
            ability.ApplyEffect(this);
        }

        if (other.transform.parent != null && other.transform.parent.GetComponent<Laser>() != null || other.transform.GetComponent<Meteorite>() != null)
        {
            GameController.Instance.ChangeState(GameManager.Dead);
        }
    }
}
