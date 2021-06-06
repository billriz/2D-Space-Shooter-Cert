using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerUpId; // 0 = triple shot; 1 = Speed Boost; 2 = Shields 3 = Laser Recharge  4 = Ship Repair 5 = Photon Blast
    [SerializeField]
    private AudioClip _PowerSoundClip;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8.0f)
        {

            Destroy(this.gameObject);
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                AudioSource.PlayClipAtPoint(_PowerSoundClip, transform.position);

                switch(_powerUpId)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.PlayerShieldActive();
                        break;
                    case 3:
                        player.LaserRecharge();
                        break;
                    case 4:
                        player.ShipRepair();
                        break;
                    case 5:
                        player.PhotonBlastActive();
                        break;
                    default:
                        Debug.LogError("No Case Found");
                        break;

                }
               

            }

            Destroy(this.gameObject);

        }

    }


}
