namespace SumaCrud2429805
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editResultadoId;

        public MainPage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            Task.Run(async () => listview.ItemsSource = await _dbService.GetResultado());
        }

        private async void sumarBtn_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(Entryprimernumero.Text, out int numero1) && int.TryParse(Entrysegundonumero.Text, out int numero2))
            {
                int suma = numero1 + numero2;
                labelresultado.Text = $"Resultado: {suma}";

                if (_editResultadoId == 0)
                {
                    await _dbService.Create(new Resultado
                    {
                        Numero1 = Entryprimernumero.Text,
                        Numero2 = Entrysegundonumero.Text,
                        Suma = suma.ToString()
                    });
                }
                else
                {
                    await _dbService.Update(new Resultado
                    {
                        Id = _editResultadoId,
                        Numero1 = Entryprimernumero.Text,
                        Numero2 = Entrysegundonumero.Text,
                        Suma = suma.ToString()
                    });
                    _editResultadoId = 0;
                }

                Entryprimernumero.Text = string.Empty;
                Entrysegundonumero.Text = string.Empty;
                listview.ItemsSource = await _dbService.GetResultado();
            }
            else
            {
                await DisplayAlert("Error", "Por favor, ingresa números válidos.", "OK");
            }
        }

        private async void listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var resultado = (Resultado)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    _editResultadoId = resultado.Id;
                    Entryprimernumero.Text = resultado.Numero1;
                    Entrysegundonumero.Text = resultado.Numero2;
                    labelresultado.Text = $"Resultado: {resultado.Suma}";
                    break;

                case "Delete":
                    await _dbService.Delete(resultado);
                    listview.ItemsSource = await _dbService.GetResultado();
                    break;
            }
        }
    }
}
