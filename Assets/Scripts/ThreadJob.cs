using System.Threading;
using System.Collections;

public class ThreadJob
{

    private bool _KeepAwake = false;
    private bool _IsDone = false;
    private object _Handle_IsDone = new object();
    private object _Handle_KeepAwake = new object();
    private Thread _Thread = null;

    public bool KeepAwake
    {
        set
        {
            lock (_Handle_KeepAwake)
            {
                _KeepAwake = value;
            }
        }
        get
        {
            bool tmp;
            lock (_Handle_KeepAwake)
            {
                tmp = _KeepAwake;
            }
            return tmp;
        }
    }
    
    public bool IsDone
    {
        get
        {
            bool tmp;
            lock (_Handle_IsDone)
            {
                tmp = _IsDone;
            }
            return tmp;
        }
        set
        {
            lock (_Handle_IsDone)
            {
                _IsDone = value;
            }
        }
    }

    public virtual void Start()
    {
        _Thread = new Thread(Run);
        _Thread.Start();
    }

    public virtual void Abort()
    {
        _Thread.Abort();
    }

    protected virtual void ThreadFunction() { }

    protected virtual void OnFinished() { }

    public virtual bool Update()
    {
        if (IsDone)
        {
            OnFinished();
            return true;
        }
        return false;
    }

    public IEnumerator WaitFor()
    {
        while (!Update())
        {
            yield return null;
        }
    }

    private void Run()
    {
        if (KeepAwake)
            DoJob_KeepAwake();
        else
            DoJob();
    }


    public void Redo()
    {
        if (KeepAwake)
            if (IsDone)
                IsDone = false;
    }

    private void DoJob()
    {
        IsDone = false;
        ThreadFunction();
        IsDone = true;
    }

    private void DoJob_KeepAwake() {
        while (KeepAwake)
        {
            IsDone = false;
            ThreadFunction();
            IsDone = true;
            while (IsDone) { Thread.Sleep(1); }
        }
    }
}
