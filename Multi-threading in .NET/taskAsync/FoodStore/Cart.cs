using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FoodStore
{
	class Cart : INotifyPropertyChanged
	{
		private ConcurrentBag<Food> cartList;

		public Cart()
		{
			cartList = new ConcurrentBag<Food>();
		}

		public decimal Sum
		{
			get
			{
				return cartList.Sum(f => f.Price);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public async Task Add(Food food)
		{
			await Task.Run(() =>
			{
				Thread.Sleep(2000);
				cartList.Add(food);
				OnPropertyChanged("Sum");
			});
		}

		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
