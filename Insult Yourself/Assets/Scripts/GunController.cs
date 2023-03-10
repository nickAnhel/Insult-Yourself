using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public string weaponName;
    public GameObject bullet;
    public Transform shotPoint;
    public GameObject shotRageRange;
    public TextMeshProUGUI bulletCounter;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public float offset;

    public int ammo;
    private int currentAmmo;
    public float reloadTime;
    private bool isReloading = false;
    public string shootSoundName;
    public string reloadStartSoundName;
    public string reloadEndSoundName;

    //public GameObject fireParticle;
    public ParticleSystem fireParticle;
    private Animator camAnim;

    public Animator anim;

    [SerializeField] private CursorManager.CursorType ShootCursorType;
    [SerializeField] private CursorManager.CursorType ReloadingCursorType;

    void Start()
    {
        bulletCounter = FindObjectOfType<TextMeshProUGUI>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        currentAmmo = ammo;
    }

    void OnEnable()
    {
        isReloading = false;
        anim.SetBool("Reloading", false);
    }

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        Vector3 LocalScale = transform.localScale;

        if (rotZ > 90 || rotZ < -90)
        {
            if (LocalScale.y > 0)
            {
                LocalScale.y *= -1;
            }
        }
        else if (LocalScale.y < 0)
        {
            LocalScale.y *= -1;
        }

        transform.localScale = LocalScale;

        if (isReloading)
        {
            return;
        }

        if ((currentAmmo <= 0) || (Input.GetKeyDown(KeyCode.R) && currentAmmo != ammo))
        {
            StartCoroutine(Reload());
            return;
        }

        Shoot();
        bulletCounter.GetComponent<ChangeText>().ChangeStr(currentAmmo);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        FindObjectOfType<AudioManager>().Play(reloadStartSoundName);
        anim.SetBool("Reloading", true);
        CursorManager.Instance.SetActiveCursorType(ReloadingCursorType);

        yield return new WaitForSeconds(reloadTime);

        anim.SetBool("Reloading", false);
        currentAmmo = ammo;
        FindObjectOfType<AudioManager>().Play(reloadEndSoundName);
        isReloading = false;
        CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Default);
    }


    void Shoot()
    {
        if (timeBtwShots <= 0)
        {
            CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Default);
            if (Input.GetMouseButton(0))
            {
                currentAmmo--;
                shotRageRange.SetActive(true);
                CursorManager.Instance.SetActiveCursorType(ShootCursorType);
                camAnim.SetTrigger("Shake");
                FindObjectOfType<AudioManager>().Play(shootSoundName);
                Instantiate(bullet, shotPoint.position, transform.rotation);
                //Instantiate(fireParticle, shotPoint.position, transform.rotation);
                fireParticle.Play();
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            if (timeBtwShots*2 < startTimeBtwShots)
            {
                shotRageRange.SetActive(false);
            }
            timeBtwShots -= Time.deltaTime;
        }
    }
}
