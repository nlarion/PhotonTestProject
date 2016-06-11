using UnityEngine;
using System.Collections;

public class LerpRigidbody:Photon.MonoBehaviour {
    // Network snapping
    protected float snapDistance = 1.5f;
    protected float snapAngle = 30f;
    protected float lerpSpeed = 20f;

    protected Collider2D col;
    protected Rigidbody2D rb;
    private Vector3 playerPosActual;
    private Quaternion playerRotActual;

    public virtual void Awake() {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Update() {
		if (photonView.isMine) {
			SelfMovement ();
		} else {
			Move (playerPosActual, playerRotActual);
		}
    }

    protected virtual void SelfMovement() {
        // Override this and call Move()
    }

    protected void Move(Vector2 newPos, Quaternion newRot) {
		if (photonView.isMine) {
			rb.MovePosition (newPos);
			rb.MoveRotation (newRot.eulerAngles.z);
		} else {
			Vector3 lerpPos = Vector3.Lerp (transform.position, newPos, Time.deltaTime * lerpSpeed);
			rb.MovePosition (lerpPos);
			Quaternion lerpRot = Quarternion.Lerp (transform.rotation, newRot, Time.deltaTime * lerpSpeed);
			rb.MoveRotation (lerpRot.eulerAngles.z);

		}
    }

    protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            playerPosActual = (Vector3) stream.ReceiveNext();
            playerRotActual = (Quaternion) stream.ReceiveNext();
        }
    }
}
