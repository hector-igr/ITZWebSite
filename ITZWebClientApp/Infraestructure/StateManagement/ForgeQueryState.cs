using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITZWebClientApp.Infraestructure.StateManagement
{
	public class ForgeQueryState
	{
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

		public string Property { get; set; }

        public event Action OnChange;

        public void SetProperty(string value)
        {
            Property = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
