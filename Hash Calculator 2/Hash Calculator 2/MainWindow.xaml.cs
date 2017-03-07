using System;
using System.Collections.Generic;
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

namespace Hash_Calculator_2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Thickness DEFAULT_MARGIN = new Thickness(5, 5, 5, 5);
		double HASH_TEXTBOX_HEIGHT = 50;

		TextBox filePath;
		TextBox compareContent;


		Dictionary<HashAlgorithm, GuiRow> guiRows = new Dictionary<HashAlgorithm, GuiRow>();

		public MainWindow()
		{
			InitializeComponent();

			DockPanel dock = InitializeDock();
			mainWindow.Content = dock;

			StackPanel mainStackPanel = InitializeStackPanel();
			DockPanel.SetDock(mainStackPanel, Dock.Bottom);
			dock.Children.Add(mainStackPanel);

			Menu menu = InitializeMenu();
			PopulateMenu(menu);
			dock.Children.Add(menu);

			Grid upperGrid = InitializeUpperGrid();
			PopulateUpperGrid(upperGrid);
			mainStackPanel.Children.Add(upperGrid);

			mainStackPanel.Children.Add(new Separator());

			Grid mainGrid = InitializeMainGrid();
			PopulateMainGrid(mainGrid);
			mainStackPanel.Children.Add(mainGrid);

			mainStackPanel.Children.Add(new Separator());

			Grid lowerGrid = InitializeLowerGrid();
			PopulateLowerGrid(lowerGrid);
			mainStackPanel.Children.Add(lowerGrid);
		}

		private DockPanel InitializeDock()
		{
			Thickness DOCK_MARGIN = new Thickness(0, 0, 10, 0);
			DockPanel dock = new DockPanel();
			dock.Margin = DOCK_MARGIN;
			return dock;
		}

		private StackPanel InitializeStackPanel()
		{
			Thickness STACKPANEL_MARGIN = new Thickness(10, 10, 0, 0);
			StackPanel mainStackPanel = new StackPanel();
			mainStackPanel.Margin = STACKPANEL_MARGIN;
			return mainStackPanel;
		}

		private Menu InitializeMenu()
		{
			Menu menu = new Menu();
			DockPanel.SetDock(menu, Dock.Top);
			menu.Background = System.Windows.SystemColors.WindowBrush;
			return menu;
		}

		private void PopulateMenu(Menu menu)
		{
			MenuItem file = new MenuItem();
			file.Header = "_File";

			MenuItem saveHashFile = new MenuItem();
			saveHashFile.Header = "_Save hash files";
			saveHashFile.IsEnabled = false;
			file.Items.Add(saveHashFile);

			file.Items.Add(new Separator());

			MenuItem exit = new MenuItem();
			exit.Header = "_Exit";
			exit.InputGestureText = "ALT+F4";
			file.Items.Add(exit);

			menu.Items.Add(file);

			MenuItem tools = new MenuItem();
			tools.Header = "_Tools";

			MenuItem options = new MenuItem();
			options.Header = "_Options";
			tools.Items.Add(options);

			menu.Items.Add(tools);

			MenuItem help = new MenuItem();
			help.Header = "_?";

			MenuItem about = new MenuItem();
			about.Header = "_About";
			help.Items.Add(about);

			menu.Items.Add(help);
		}

		private void SetColumnDefinition(Grid grid)
		{
			GridLength LEFT_COLUMN = new GridLength(2, GridUnitType.Star);
			GridLength MIDDLE_COLUMN = new GridLength(12, GridUnitType.Star);
			GridLength RIGHT_COLUMN = new GridLength(2, GridUnitType.Star);

			ColumnDefinition leftColumn = new ColumnDefinition();
			ColumnDefinition middleColumn = new ColumnDefinition();
			ColumnDefinition rightColumn = new ColumnDefinition();
			leftColumn.Width = LEFT_COLUMN;
			middleColumn.Width = MIDDLE_COLUMN;
			rightColumn.Width = RIGHT_COLUMN;
			grid.ColumnDefinitions.Add(leftColumn);
			grid.ColumnDefinitions.Add(middleColumn);
			grid.ColumnDefinitions.Add(rightColumn);
		}

		private Grid InitializeUpperGrid()
		{
			Grid upperGrid = new Grid();
			SetColumnDefinition(upperGrid);

			upperGrid.RowDefinitions.Add(new RowDefinition());
			upperGrid.RowDefinitions.Add(new RowDefinition());

			return upperGrid;
		}

		private void PopulateUpperGrid(Grid upperGrid)
		{
			Label fileOpen = new Label();
			fileOpen.Content = "File";
			fileOpen.Margin = DEFAULT_MARGIN;
			Grid.SetRow(fileOpen, 0);
			Grid.SetColumn(fileOpen, 0);
			upperGrid.Children.Add(fileOpen);

			filePath = new TextBox();
			filePath.Margin = DEFAULT_MARGIN;
			Grid.SetRow(filePath, 0);
			Grid.SetColumn(filePath, 1);
			upperGrid.Children.Add(filePath);

			Button open = new Button();
			open.Content = "Open";
			open.Margin = DEFAULT_MARGIN;
			Grid.SetRow(open, 0);
			Grid.SetColumn(open, 2);
			upperGrid.Children.Add(open);

			Label compare = new Label();
			compare.Content = "Compare hash\nto results";
			compare.Height = HASH_TEXTBOX_HEIGHT;
			compare.Margin = DEFAULT_MARGIN;
			Grid.SetRow(compare, 1);
			Grid.SetColumn(compare, 0);
			upperGrid.Children.Add(compare);

			compareContent = new TextBox();
			compareContent.Margin = DEFAULT_MARGIN;
			Grid.SetRow(compareContent, 1);
			Grid.SetColumn(compareContent, 1);
			upperGrid.Children.Add(compareContent);

			Image compareTip = new Image();
			BitmapImage compareTipImage = new BitmapImage();
			compareTipImage.BeginInit();
			compareTipImage.UriSource = new Uri("Resources/Information.png", UriKind.Relative);
			compareTipImage.EndInit();
			compareTip.Source = compareTipImage;
			compareTip.Height = 32;
			compareTip.Width = 32;
			compareTip.ToolTip = "The input is automatically converted to the internal format,\nso you don't need to worry about capitalization or dashes";
			compareTip.Margin = DEFAULT_MARGIN;
			Grid.SetRow(compareTip, 1);
			Grid.SetColumn(compareTip, 2);
			upperGrid.Children.Add(compareTip);
		}

		private Grid InitializeMainGrid()
		{
			Grid mainGrid = new Grid();
			SetColumnDefinition(mainGrid);
			return mainGrid;
		}

		private void PopulateMainGrid(Grid mainGrid)
		{
			double PROGRESS_HEIGHT = 15;
			int i = 0;
			foreach (HashAlgorithm hashAlgorithm in Enum.GetValues(typeof(HashAlgorithm)))
			{
				mainGrid.RowDefinitions.Add(new RowDefinition());
				GuiRow guiRow = new GuiRow();

				StackPanel leftPanel = new StackPanel();
				leftPanel.Orientation = Orientation.Horizontal;

				CheckBox select = new CheckBox();
				select.IsChecked = true;
				select.VerticalAlignment = VerticalAlignment.Center;
				guiRow.Select = select;
				leftPanel.Children.Add(select);

				Label label = new Label();
				label.VerticalAlignment = VerticalAlignment.Center;
				label.Content = Enum.GetName(typeof(HashAlgorithm), hashAlgorithm);
				leftPanel.Children.Add(label);

				Grid.SetRow(leftPanel, i);
				Grid.SetColumn(leftPanel, 0);
				mainGrid.Children.Add(leftPanel);

				TextBox result = new TextBox();
				result.TextWrapping = TextWrapping.Wrap;
				result.Height = HASH_TEXTBOX_HEIGHT;
				result.IsReadOnly = true;
				result.Margin = DEFAULT_MARGIN;
				guiRow.Result = result;
				Grid.SetRow(result, i);
				Grid.SetColumn(result, 1);
				mainGrid.Children.Add(result);

				StackPanel rightPanel = new StackPanel();

				Button copy = new Button();
				copy.Content = "Copy";
				copy.Margin = DEFAULT_MARGIN;
				rightPanel.Children.Add(copy);

				ProgressBar progress = new ProgressBar();
				guiRow.Progress = progress;
				progress.Height = PROGRESS_HEIGHT;
				progress.Margin = DEFAULT_MARGIN;
				rightPanel.Children.Add(progress);

				Grid.SetRow(rightPanel, i);
				Grid.SetColumn(rightPanel, 2);
				mainGrid.Children.Add(rightPanel);
				
				guiRows.Add(hashAlgorithm, guiRow);
				i++;
			}

		}

		private Grid InitializeLowerGrid()
		{
			Grid lowerGrid = new Grid();
			SetColumnDefinition(lowerGrid);

			lowerGrid.RowDefinitions.Add(new RowDefinition());

			return lowerGrid;
		}

		private void PopulateLowerGrid(Grid lowerGrid)
		{
			Button run = new Button();
			run.Content = "Run";
			run.Margin = DEFAULT_MARGIN;
			Grid.SetRow(run, 0);
			Grid.SetColumn(run, 2);
			lowerGrid.Children.Add(run);
		}

	}
}
