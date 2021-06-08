using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vjp;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private Manager manager;
    private Option<Token> selectedToken = Option<Token>.None();

    private const float NEW_Y = 1f;

    private void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {

            if (Input.GetMouseButtonDown(0)) {
                var token = hit.transform.GetComponent<Token>();

                if (token == null) {
                    return;
                }

                selectedToken = Option<Token>.Some(token);
            }

            if (selectedToken.IsNone()) {
                return;
            }

            selectedToken.Peel().transform.position = new Vector3(hit.point.x, NEW_Y, hit.point.z);

            if (Input.GetMouseButtonUp(0)) {
                int mouseX = (int)(hit.point.x + 0.5);
                int mouseY = (int)(hit.point.z + 0.5);

                manager.MoveToken(mouseX, mouseY, selectedToken.Peel());

                selectedToken = Option<Token>.None();
            }
        }
    }
}
