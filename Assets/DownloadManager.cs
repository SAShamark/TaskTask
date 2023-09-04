using System;
using System.Threading.Tasks;
using UnityEngine;

public class DownloadManager<T>
{
    public T[] TaskResult;

    public DownloadManager(string url)
    {
        Init(url);
    }

    private async void Init(string url)
    {
        Task<T[]> tasks = DownloadAsync(url);

        await Task.WhenAll(tasks);

        TaskResult = tasks.Result;
    }

    public async Task<T[]> DownloadAsync(string url)
    {
        try
        {
            //TODO download something
            return await Todo();
        }
        catch (Exception e)
        {
            Debug.LogError($"error with URL {url}: {e.Message}");
            return null;
        }
    }

    private Task<T[]> Todo()
    {
        T[] a = Array.Empty<T>();
        return Task.FromResult(a);
    }
}