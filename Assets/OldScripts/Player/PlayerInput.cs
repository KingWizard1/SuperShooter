using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperShooterOld.PB;

namespace SuperShooterOld
{

    public class PlayerInput : MonoBehaviour
    {

        public PlayerCharacter characterModel;

        // ------------------------------------------------- //

        private int weaponIndex = 0;

        // ------------------------------------------------- //

        void Update()
        {

            // Move player the player. WASD or Arrows.
            // Vertical input = forward/back on the z axis.
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            characterModel.Move(new Vector3(horizontal, 0, vertical));

            // Player jump
            if (Input.GetKeyDown(KeyCode.Space))
                characterModel.Jump();

            // Main attack
            if (Input.GetButtonDown("Fire1"))
                characterModel.Attack();

            // Weapon Reload
            if (Input.GetKeyDown(KeyCode.R))
                characterModel.Reload();

            // Interact with world object
            if (Input.GetKeyDown(KeyCode.E))
                characterModel.Interact();

            // Switch weapons/gadgets
            // Can be done by mouse wheel, or keys 1-3 on the keyboard.
            CheckScrollWheel();
            if (Input.GetKeyDown(KeyCode.Alpha1)) characterModel.SelectWeapon(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) characterModel.SelectWeapon(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) characterModel.SelectWeapon(2);
        }

        // ------------------------------------------------- //

        void CheckScrollWheel()
        {

            var currentSlot = weaponIndex;

            // Mouse scrolled forward, or backward?
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && weaponIndex > 0)
                weaponIndex -= 1;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && weaponIndex < characterModel.weapons.Length - 1)
                weaponIndex += 1;

            if (currentSlot != weaponIndex)
                return;
            else
            {
                weaponIndex = currentSlot;
                characterModel.SelectWeapon(weaponIndex);
            }
        }
    }

}