using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Admin.Command;
using Restaurant.Admin.Model;
using Restaurant.Core.Data;

namespace Restaurant.Admin.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        //Database collections
        public ObservableCollection<Food> Foods
        {
            get { return foods; }
            private set
            {
                foods = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Order> Orders
        {
            get { return orders; }
            private set
            {
                orders = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            private set
            {
                categories = value;
                OnPropertyChanged();
            }
        }        
        private ObservableCollection<Food> foods { get; set; }
        private ObservableCollection<Order> orders { get; set; }
        private ObservableCollection<Category> categories { get; set; }

        private Order selectedOrder;
        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                OnPropertyChanged();
            }
        }

        private Food selectedFood;        
        public Food SelectedFood
        {
            get { return selectedFood; }
            set
            {
                selectedFood = value;
                OnPropertyChanged();                
            }
        }

        private Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged();
            }
        }

        //Food edit windows
        public Food EditedFood { get; set; }
        public Order EditedOrder { get; set; }
        public Category EditedCategory { get; set; }
        public string OrderedFoodsList { get; set; }

        //Commands
        public DelegateCommand ExitCommand { get; set; }
        public DelegateCommand LoadCommand { get; set; }
        //   Food commands
        public DelegateCommand AddFoodCommand { get; set; }
        public DelegateCommand EditFoodCommand { get; set; }
        public DelegateCommand SaveFoodCommand { get; set; }
        public DelegateCommand CancelFoodCommand { get; set; }
        //   Category commands
        public DelegateCommand AddCategoryCommand { get; set; }
        public DelegateCommand EditCategoryCommand { get; set; }
        public DelegateCommand SaveCategoryCommand { get; set; }
        public DelegateCommand DeleteCategoryCommand { get; set; }
        public DelegateCommand CancelCategoryCommand { get; set; }
        //   Order commands
        public DelegateCommand MarkOrderCompleteCommand { get; set; }
        public DelegateCommand EditOrderCommand { get; set; }
        public DelegateCommand CloseOrderCommand { get; set; }

        //Events
        public event EventHandler OrderEditStarted;
        public event EventHandler FoodEditStarted;
        public event EventHandler CategoryEditStarted;
        public event EventHandler OrderEditEnded;
        public event EventHandler FoodEditEnded;
        public event EventHandler CategoryEditEnded;

        private RestaurantModel model;

        public MainViewModel(RestaurantModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            this.model = model;

            LoadCommand = new DelegateCommand(p => LoadAsync());
            ExitCommand = new DelegateCommand(p => LoadAsync());

            //Food
            AddFoodCommand = new DelegateCommand(p => AddFood());
            CancelFoodCommand = new DelegateCommand(p => CancelFood());
            SaveFoodCommand = new DelegateCommand(p => SaveFood());
            EditFoodCommand = new DelegateCommand(p => EditFood(p as Food));

            //Order
            EditOrderCommand = new DelegateCommand(p => EditOrder(p as Order));
            MarkOrderCompleteCommand = new DelegateCommand(p => OrderComplete());
            CloseOrderCommand = new DelegateCommand(p => CancelOrder());

            //Category
            AddCategoryCommand = new DelegateCommand(p => AddCategory());
            CancelCategoryCommand = new DelegateCommand(p => CancelCategory());
            SaveCategoryCommand = new DelegateCommand(p => SaveCategory());
            EditCategoryCommand = new DelegateCommand(p => EditCategory(p as Category));
            DeleteCategoryCommand = new DelegateCommand(p => DeleteCategory(p as Category));

            LoadAsync();
        }

        private async void LoadAsync()
        {
            try
            {
                await model.LoadAsync();
                Foods = new ObservableCollection<Food>(model.Foods);
                Orders = new ObservableCollection<Order>(model.Orders);
                Categories = new ObservableCollection<Category>(model.Categories);
            }
            catch (Exception e)
            {
                OnMessageApplication("Cannot load data.");
            }
        }

        //Food
        private void AddFood()
        {
            EditedFood = new Food();

            if (FoodEditStarted != null)
                FoodEditStarted(this, null);
        }
        private void EditFood(Food food)
        {
            EditedFood = new Food
            {
                Name = food.Name,
                Description = food.Description,
                Id = food.Id,
                OrderCount = food.OrderCount,
                Price = food.Price,
                Spicy = food.Spicy,
                Type = food.Type,
                Vegetarian = food.Vegetarian,
            };

            if (FoodEditStarted != null)
                FoodEditStarted(this, null);
        }
        private async Task SaveFood()
        {
            //Guards
            if (string.IsNullOrEmpty(EditedFood.Name))
            {
                MessageBox.Show("Name cannot be empty!");
                return;
            }
            if (EditedFood.Price == 0)
            {
                MessageBox.Show("Price cannot be 0!");
                return;
            }
            if (string.IsNullOrEmpty(EditedFood.Type))
            {
                MessageBox.Show("Type cannot be empty!");
                return;
            }

            //Select type
            if (EditedFood.Id == 0) //New food
            {
                await model.AddFoodAsync(EditedFood);
                Foods.Add(EditedFood);
                SelectedFood = EditedFood;
            }
            else //Update food
            {
                await model.UpdateFoodAsync(EditedFood);
                SelectedFood = EditedFood;
            }

            EditedFood = null;
            FoodEditEnded(this, null);
            LoadCommand.Execute(null);
        }
        private void CancelFood()
        {
            EditedFood = null;
            FoodEditEnded(this, null);
        }

        //Order
        private void EditOrder(Order order)
        {
            OrderedFoodsList = string.Empty;
            //Get ordered food names
            var orderedFoods = model.OrderedFoods.Where(i => i.OrderId == order.Id).ToList();
            foreach (var of in orderedFoods)
            {
                OrderedFoodsList += model.Foods.Where(i => i.Id == of.FoodId).First().Name + "\n";
            }

            EditedOrder = new Order
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                Completed = order.Completed,
                Date = order.Date,
                Phone = order.Phone,
                Price = order.Price,
            };

            if (OrderEditStarted != null)
                OrderEditStarted(this, null);
        }
        private async Task OrderComplete()
        {
            //Update db
            await model.UpdateOrderAsync(EditedOrder);
            SelectedOrder = EditedOrder;

            EditedOrder = null;
            OrderEditEnded(this, null);
            LoadCommand.Execute(null);
        }
        private void CancelOrder()
        {
            EditedOrder = null;
            OrderEditEnded(this, null);
        }

        //Category
        private void AddCategory()
        {
            EditedCategory = new Category();

            if (CategoryEditStarted != null)
                CategoryEditStarted(this, null);
        }
        private void EditCategory(Category category)
        {
            EditedCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                ComplexList = category.ComplexList,
                Link = category.Link,
                TypeName = category.TypeName,
            };

            if (CategoryEditStarted != null)
                CategoryEditStarted(this, null);
        }
        private async Task SaveCategory()
        {
            //Guards
            if (string.IsNullOrEmpty(EditedCategory.Name))
            {
                MessageBox.Show("Name cannot be empty!");
                return;
            }
            if (string.IsNullOrEmpty(EditedCategory.Link))
            {
                MessageBox.Show("Link cannot be empty!");
                return;
            }
            if (string.IsNullOrEmpty(EditedCategory.TypeName))
            {
                MessageBox.Show("Type cannot be empty!");
                return;
            }

            //Select type
            if (EditedCategory.Id == 0) //New category
            {
                await model.AddCategoryAsync(EditedCategory);
                Categories.Add(EditedCategory);
                SelectedCategory = EditedCategory;
            }
            else //Update category
            {
                await model.UpdateCategoryAsync(EditedCategory);
                SelectedCategory = EditedCategory;
            }

            EditedCategory = null;
            CategoryEditEnded(this, null);
            LoadCommand.Execute(null);
        }
        private async Task DeleteCategory(Category category)
        {
            if (category == null) return;

            await model.DeleteCategoryAsync(category);
            LoadCommand.Execute(null);
        }
        private void CancelCategory()
        {
            EditedCategory = null;
            CategoryEditEnded(this, null);
        }
    }
}
