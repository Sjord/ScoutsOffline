using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ScoutsOffline.Sol
{
    public class GetMembersWorker : BackgroundWorker
    {
        private ScoutsOnLine sol;

        public GetMembersWorker(ScoutsOnLine sol)
        {
            this.sol = sol;
            this.WorkerReportsProgress = this.WorkerSupportsCancellation = true;
        }

        protected override void OnDoWork(DoWorkEventArgs eventArgs)
        {
            base.OnDoWork(eventArgs);

            int i = 0;
            int count = sol.roles.Count;
            foreach (var role in sol.roles)
            {
                var percent = 100 * ++i / count;
                try
                {
                    var members = sol.GetSelection(role);
                    this.ReportProgress(percent, members);
                }
                catch (Exception e)
                {
                    this.ReportProgress(percent, e);
                }
                if (this.CancellationPending) return;
            }
        }
    }
}
