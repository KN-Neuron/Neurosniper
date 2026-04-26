using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

public class EEGManager : MonoBehaviour
{
    public static EEGManager Instance { get; private set; }

    public int PoorSignal { get; private set; } = -1;
    public int Attention { get; private set; } = -1;
    public int Meditation { get; private set; } = -1;
    public bool IsConnected { get; private set; } = false;

    private TcpClient client;
    private StreamReader reader;
    private StreamWriter writer;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        try
        {
            client = new TcpClient("127.0.0.1", 13854); // Connect to TGC
            var stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream, Encoding.ASCII);

            Debug.Log("ThinkGearConnector is working - turn on the EEG device");

            writer.WriteLine("{\"enableRawOutput\": false, \"format\": \"Json\"}");
            writer.Flush();
        }
        catch (Exception e)
        {
            IsConnected = false;
            Debug.LogError("ThinkGearConnector is not working: " + e.Message);
        }
    }

    private async void Update()
    {
        if (client != null && client.Available > 0)
        {
            var json = await reader.ReadLineAsync();
            if (json != null)
            {
                ParseEEGData(json);
            }
        }
    }

    private void ParseEEGData(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        try
        {
            var data = JsonConvert.DeserializeObject<EEGDataWrapper>(json);
            if (data != null)
            {

                if (data.poorSignalLevel == 0)
                {
                    // Dopiero teraz stwierdzamy, że urządzenie jest AKTYWNE
                    if (!IsConnected)
                    {
                        Debug.Log("EEG Device connected and signal is GOOD (PoorSignal = 0).");
                    }
                    IsConnected = true;
                }
                else
                {
                    // Urządzenie jest podłączone do TGC, ale nie ma sygnału (nie na głowie/wyłączone)
                    if (IsConnected)
                    {
                        Debug.LogWarning("EEG signal lost (PoorSignal > 0).");
                    }
                    IsConnected = false;
                }
                // -----------------------------

                PoorSignal = data.poorSignalLevel;

                if (data.eSense != null)
                {
                    Attention = data.eSense.attention;
                    Meditation = data.eSense.meditation;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to parse EEG JSON: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        reader?.Close();
        writer?.Close();
        client?.Close();
    }

    [Serializable]
    public class EEGDataWrapper
    {
        public ESense eSense;
        public int poorSignalLevel;
    }

    [Serializable]
    public class ESense
    {
        public int attention;
        public int meditation;
    }
}