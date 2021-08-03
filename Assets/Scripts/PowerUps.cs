using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerUpId; // 0 = triple shot; 1 = Speed Boost; 2 = Shields 3 = Laser Recharge  4 = Ship Repair 5 = Photon Blast 6 = Negitive
    [SerializeField]
    private AudioClip _PowerSoundClip;
    [SerializeField]
    private GameObject _explosionPreFab;

    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            PlayerIsCollecting();
        }
        else
        {
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
        }
        

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
                    case 6:
                        player.NegitiveBoostActive();
                        break;
                    default:
                        Debug.LogError("No Case Found");
                        break;
                }               

            }

            Destroy(this.gameObject);

        }

        if (other.tag == "Laser")
        {
            Instantiate(_explosionPreFab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    void PlayerIsCollecting()
    {
        _player = GameObject.Find("Player");
        Vector3 _direction = this.transform.position - _player.transform.position;
        _direction = _direction.normalized;
        this.transform.position -= _direction * Time.deltaTime * (_speed * 3);

    }


}
