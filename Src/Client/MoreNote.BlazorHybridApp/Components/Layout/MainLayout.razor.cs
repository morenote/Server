using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.BlazorHybridApp.Components.Layout
{
    public partial class MainLayout
    {
        bool loading = true;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Task.Run(async () => {
                await Task.Delay(1000);
                loading = false;
                await this.InvokeAsync(() => {

                    StateHasChanged();
                });
              
            });
        }
    }
}
