using System.Threading.Tasks;

public class ExampleDownloadManager<T1, T2>
{
    public T1[] TaskResult1;
    public T2[] TaskResult2;

    public ExampleDownloadManager(string url1, string url2)
    {
        Init(url1, url2);
    }

    private async void Init(string url1, string url2)
    {
        DownloadManager<T1> manager1=new (url1);
        Task<T1[]> tasks1 = manager1.DownloadAsync(url1);
        
        DownloadManager<T2> manager2=new (url2);
        Task<T2[]> tasks2 = manager2.DownloadAsync(url2);

        await Task.WhenAll(tasks1,tasks2);

        TaskResult1 = tasks1.Result;
        TaskResult2 = tasks2.Result;
    }
}