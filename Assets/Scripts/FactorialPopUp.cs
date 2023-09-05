using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactorialPopUp : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_Text _answerText;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Image _image;
    [SerializeField] private Button _calculateButton;
    [SerializeField] private Button _downloadImageButton;
    [SerializeField] private Button _downloadMusicButton;
    [SerializeField] private float _maxExecutionTime = 1;

    private CancellationTokenSource _cancellationTokenSource;
    private long _result;

    private void Awake()
    {
        _calculateButton.onClick.AddListener(() => CalculateFactorialAsync(int.Parse(_inputField.text)));

        ExampleDownloadManager<AudioClip, Sprite> downloadManager = new("url.mp3", "url.jpg");
        _downloadMusicButton.onClick.AddListener(() => _audioSource.clip = downloadManager.TaskResult1[0]);
        _downloadImageButton.onClick.AddListener(() => _image.sprite = downloadManager.TaskResult2[0]);
    }

    private async void CalculateFactorialAsync(int number)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        try
        {
            long result = await CalculateFactorialAsync(number, _cancellationTokenSource.Token);
            Debug.Log($"Factorial {number} = {result}");
            _answerText.text = result.ToString();
        }
        catch (Exception exception)
        {
            Debug.LogWarning(exception);
            throw;
        }
    }

    private async Task<long> CalculateFactorialAsync(int number, CancellationToken cancellationToken)
    {
        switch (number)
        {
            case < 0:
                _answerText.text = "The number must be non-negative";
                throw new ArgumentException("The number must be non-negative.");
            case 0:
                return 1;
        }

        long result = 1;
        DateTime startTime = DateTime.Now;

        for (int i = 1; i <= number; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result *= i;

            TimeSpan elapsedTime = DateTime.Now - startTime;
            if (elapsedTime.TotalSeconds > _maxExecutionTime)
            {
                Cancel();
            }

            await Task.Delay(100, cancellationToken); //imitate operation time
        }

        return result;
    }

    private void Cancel()
    {
        _cancellationTokenSource?.Cancel();
        _answerText.text = "The factorial calculation was canceled after too much time.";
    }
}