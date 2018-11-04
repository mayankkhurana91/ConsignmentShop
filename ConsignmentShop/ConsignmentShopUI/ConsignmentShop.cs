using ConsignmentShopLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentShopUI
{
    public partial class ConsignmentShop : Form
    {
        private Store store = new Store();
        private List<Item> shoppingCartData = new List<Item>();
        BindingSource itemsBinding = new BindingSource(); // For displaying store items (itemsListBox)
        BindingSource cartBinding = new BindingSource(); // For displaying shopping cart (shoppingCartListBox)
        BindingSource vendorsBinding = new BindingSource(); // For displaying vendors (vendorListBox)
        public decimal storeProfit = 0;

        public ConsignmentShop()
        {   
            InitializeComponent();
            SetupData();

            // itemsListBox

            //itemsBinding.DataSource = store.Items;
            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();
            itemsListBox.DataSource = itemsBinding;

            // Displaying Items in a specific format : Item -> Price, Display property is defined in Item Class

            itemsListBox.DisplayMember = "Display";
            itemsListBox.ValueMember = "Display";

            // shoppingCartListBox

            cartBinding.DataSource = shoppingCartData;
            shoppingCartListBox.DataSource = cartBinding;

            // Displaying Items in a specific format : Item -> Price, Display property is defined in Item Class

            shoppingCartListBox.DisplayMember = "Display";
            itemsListBox.ValueMember = "Display";

            // vendorListBox

            vendorsBinding.DataSource = store.Vendors;
            vendorListBox.DataSource = vendorsBinding;

            vendorListBox.DisplayMember = "Display";
            vendorListBox.ValueMember = "Display";
        }

        private void SetupData()
        {
            //Vendor demoVendor = new Vendor();
            //demoVendor.FirstName = "Bill";
            //demoVendor.LastName = "Smith";
            //demoVendor.Commission = 0.5;

            //store.Vendors.Add(demoVendor);

            //demoVendor = new Vendor();
            //demoVendor.FirstName = "Sue";
            //demoVendor.LastName = "Jones";
            //demoVendor.Commission = 0.5;

            store.Vendors.Add(new Vendor { FirstName = "Bill", LastName = "Smith" });
            store.Vendors.Add(new Vendor { FirstName = "Sue", LastName = "Jones" });

            store.Items.Add(new Item
            {
                Title = "Moby Dick",
                Description = "A good book about a whale",
                Price = 4.50M,
                Owner = store.Vendors[0],
                PaymentDistributed = true
            });
            
            store.Items.Add(new Item
            {
                Title = "A tale of two cities",
                Description = "A book about a revolution",
                Price = 3.80M,
                Owner = store.Vendors[1],
                PaymentDistributed = true
            });

            store.Items.Add(new Item
            {
                Title = "Harry Potter Book 1",
                Description = "A book about a boy",
                Price = 5.20M,
                Owner = store.Vendors[1],
                PaymentDistributed = true
            });

            store.Items.Add(new Item
            {
                Title = "Jane Eyre",
                Description = "A book about a girl",
                Price = 1.50M,
                Owner = store.Vendors[0],
                PaymentDistributed = true
            });

            store.Name = "Seconds are better";

        }

        private void addToCard_Click(object sender, EventArgs e)
        {
            // Figure out what is selected from the item list
            // Copy that item to the shopping cart
            // Do we remove from the items list? - no

            Item selectedItem = (Item)itemsListBox.SelectedItem;
            //MessageBox.Show(selectedItem.Title);  

            shoppingCartData.Add(selectedItem);

            cartBinding.ResetBindings(false);

        }

        private void makePurchase_Click(object sender, EventArgs e)
        {
            // Mark each item in the cart as sold
            // Clear the cart

            //shoppingCartData

            foreach (Item item in shoppingCartData)
            {
                item.Sold = true;
                item.Owner.PaymentDue += (decimal)item.Owner.Commission * item.Price; 
                storeProfit += (1 - (decimal)item.Owner.Commission) * item.Price;
            }

            shoppingCartData.Clear();

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();

            storeProfitValue.Text = string.Format("${0}", storeProfit);

            cartBinding.ResetBindings(false);
            itemsBinding.ResetBindings(false);
            vendorsBinding.ResetBindings(false);
        }
    }
}
