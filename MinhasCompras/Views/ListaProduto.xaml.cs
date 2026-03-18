using MinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        try { 

            await CarregarProdutos();

        }catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
        
    }

    async Task CarregarProdutos()
    {
        try {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
        
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {

        try {

            string q = e.NewTextValue;

            if (string.IsNullOrWhiteSpace(q))
            {
                await CarregarProdutos();
                return;
            }

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        DisplayAlert("Total", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try {
            var menuItem = sender as MenuItem;

            var produto = menuItem?.BindingContext as Produto;

            if (produto == null)
                return;

            bool confirm = await DisplayAlert("Confirmar", "Deseja excluir este produto?", "Sim", "Não");

            if (!confirm)
                return;

            await App.Db.Delete(produto.Id);

            lista.Remove(produto);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {

            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto{BindingContext = p});

        }
        catch    (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}