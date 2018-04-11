using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FoodStore
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		FoodContext foodContext;
		Cart cart;

		public MainWindow()
		{
			InitializeComponent();

			cart = new Cart();
			foodContext = new FoodContext();
			foodContext.Foods.Load();
			this.DataContext = new
			{
				FoodList = foodContext.Foods.Local.ToBindingList(),
				Cart = cart
			};
		}

		private async void Add_Click(object sender, RoutedEventArgs e)
		{
			if (foodList.SelectedItem == null)
			{
				return;
			}
			
			Food food = foodList.SelectedItem as Food;
			await cart.Add(food);
		}
	}
}
