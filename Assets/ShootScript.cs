using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ShootScript : MonoBehaviour
{
    // Hint: By making the prefab field Rigidbody you later can skip GetComponent
    public Rigidbody bulletPrefab;
    public float shootSpeed = 1;
    public float offset = 1;
    public Camera MainCamera;

    private List<InputDevice> devicesWithPrimaryButton;

    void Start()
    {
        devicesWithPrimaryButton = new List<InputDevice>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shootBullet();
        }

        foreach (var device in devicesWithPrimaryButton)
        {
            device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
            if(primaryButtonValue == true)
            {
                shootBullet();
            }
        }
    }

    void shootBullet()
    {
        // Instantiate a new bullet at the players position and rotation
        // later you might want to add an offset here or 
        // use a dedicated spawn transform under the player
        var projectile = Instantiate (bulletPrefab, MainCamera.transform.position+MainCamera.transform.forward*offset, MainCamera.transform.rotation);
    
        //Shoot the Bullet in the forward direction of the player
        projectile.GetComponent<Rigidbody>().velocity = MainCamera.transform.forward * shootSpeed;
    }
}