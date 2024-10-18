using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketManager : MonoBehaviour
{
    public GameObject mangkok; // Objek mangkok yang memiliki socket
    public GameObject PrefabSayuran; // Prefab tomat yang akan ditampilkan
    public Transform posisiSayuran; // Lokasi di mana tomat akan muncul, misalnya di atas talenan

    private XRSocketInteractor socketInteractor; // Socket Interactor pada mangkok

    private void Start()
    {
        // Ambil komponen XRSocketInteractor dari mangkok
        socketInteractor = mangkok.GetComponent<XRSocketInteractor>();

        // Event listener untuk deteksi saat wortel dimasukkan ke socket
        socketInteractor.selectEntered.AddListener(OnSayuranMasukSocket);
    }

    // Fungsi untuk menangani event saat wortel dimasukkan ke dalam socket
    private void OnSayuranMasukSocket(SelectEnterEventArgs args)
    {
        GameObject Sayuran = args.interactableObject.transform.gameObject;

        // Pastikan objek yang dimasukkan adalah wortel
        if (Sayuran.CompareTag("Sayuran"))
        {
            // Spawn tomat di posisi yang ditentukan (misalnya di talenan)
            Instantiate(PrefabSayuran, posisiSayuran.position, posisiSayuran.rotation);
        }
    }
}
