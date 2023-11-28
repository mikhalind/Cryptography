using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Tab
{
    public partial class InputWindow : Window
    {
        int currentButtonCount;
        private static int rowCount;
        private static int colCount;
        public string key;
        List<List<Button>> matrix;

        public void RowsChangeEnabled(bool variable)
        {
            addRowBtn.IsEnabled = variable;
            delRowBtn.IsEnabled = variable;
        }

        public void ColsChangeEnabled(bool variable)
        {
            addColBtn.IsEnabled = variable;
            delColBtn.IsEnabled = variable;
        }

        public InputWindow()
        {
            InitializeComponent();
            key = String.Empty;
            currentButtonCount = 0;
            matrix = new List<List<Button>>();
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.IsEnabled = false;
            key += b.Content + " ";
            if (matrix.All(row => row.All(btn => btn.IsEnabled == false)))
                this.Hide();      
            RowsChangeEnabled(false);
            ColsChangeEnabled(false);
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        public void Update(int rows, int columns)
        {
            matrix.Clear();
            key = String.Empty;
            buttonsGrid.Children.Clear();
            buttonsGrid.RowDefinitions.Clear();
            buttonsGrid.ColumnDefinitions.Clear();
            rowCount = rows;
            colCount = columns;
            this.Height = 110 + 50 * rowCount;
            this.Width = (colCount > 8) ? 400 + 50 * (colCount - 8) : 400;
            CenterWindowOnScreen();

            for (int i = 0; i < rowCount; i++)
                buttonsGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < colCount; i++)
                buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < rowCount; i++)
            {
                List<Button> stringOfButtons = new List<Button>(); ;
                for (int j = 0; j < colCount; j++)
                {
                    var newBtn = new Button();
                    stringOfButtons.Add(newBtn);
                    newBtn.Content = (i * colCount + j + 1).ToString();
                    newBtn.Click += ButtonOnClick;
                    Grid.SetRow(newBtn, i);
                    Grid.SetColumn(newBtn, j);
                    buttonsGrid.Children.Add(newBtn);
                }
                matrix.Add(stringOfButtons);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.GetPosition(this).Y < 35)
                this.DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            key = String.Empty;
            for (int i = 0; i < currentButtonCount; i++)
                key += (i + 1).ToString() + " ";
            RowsChangeEnabled(false);
            ColsChangeEnabled(false);
            this.Hide();
        }

        private void AddRowBtn_Click(object sender, RoutedEventArgs e)
        {
            Update(rowCount + 1, colCount);
        }

        private void AddColBtn_Click(object sender, RoutedEventArgs e)
        {
            Update(rowCount, colCount + 1);
        }

        private void DelRowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (rowCount == 1)
                MessageBox.Show("Нет");
            else
                Update(rowCount - 1, colCount);
        }

        private void DelColBtn_Click(object sender, RoutedEventArgs e)
        {
            if (colCount == 1)
                MessageBox.Show("Нет");
            else
                Update(rowCount, colCount - 1);
        }
    }
}
