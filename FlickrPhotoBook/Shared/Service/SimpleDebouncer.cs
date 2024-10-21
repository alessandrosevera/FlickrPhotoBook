using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlickrPhotoBook.Service
{
    public class SimpleDebouncer<T>
    {
        #region enums

        private enum DebouncerState
        {
            Idle,
            Acting
        }

        #endregion

        #region auto-properties

        private CancellationToken CancellationToken { get; }
        private Func<string, Task<IList<T>>> Retriever { get; }
        private Func<IList<T>, Task> Setter { get; }

        private TimeSpan Interval { get; }

        private object LockObject { get; }
        private DebouncerState State { get; set; }
        private int QueryMinLength { get; }
        private string LastRequest { get; set; }
        private DateTime LastRequestTimestamp { get; set; }

        #endregion

        #region ctor(s)

        public SimpleDebouncer(CancellationToken cancellationToken, Func<string, Task<IList<T>>> retriever,
            Func<IList<T>, Task> setter, TimeSpan interval, int queryMinLength)
        {
            CancellationToken = cancellationToken;
            Retriever = retriever;
            Interval = interval;
            Setter = setter;
            QueryMinLength = queryMinLength;

            LockObject = new object();
        }

        #endregion


        #region access methods

        public void Debounce(string query)
        {
            lock (LockObject)
            {
                LastRequestTimestamp = DateTime.Now;
                LastRequest = query;

                if (State == DebouncerState.Idle)
                {
                    Task.Run(() => Run(query));
                }
            }
        }

        #endregion


        #region helper methods

        private void SetState(DebouncerState state)
        {
            lock (LockObject)
            {
                System.Diagnostics.Debug.WriteLine("******** " + state);

                State = state;
            }
        }

        private async Task Run(string query)
        {
            SetState(DebouncerState.Acting);

            string lastQuery = null;

            while (!CancellationToken.IsCancellationRequested)
            {
                IList<T> result;

                if (lastQuery != query)
                {
                    if (query.Length >= QueryMinLength)
                    {
                        try
                        {
                            result = await Retriever.Invoke(query);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex);
                            result = new T[] { };
                        }
                    }
                    else
                    {
                        result = new T[] { };
                    }

                    await Setter.Invoke(result);
                    lastQuery = query;
                }

                if (DateTime.Now - LastRequestTimestamp > Interval)
                {
                    if (query == LastRequest)
                    {
                        break;
                    }
                    else
                    {
                        query = LastRequest;
                    }
                }
                else
                {
                    await Task.Delay(100);
                }

            }

            SetState(DebouncerState.Idle);
        }

        #endregion
    }
}

