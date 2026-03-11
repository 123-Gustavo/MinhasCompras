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

        await CarregarProdutos();
    }

    async Task CarregarProdutos()
    {
        lista.Clear();

        List<Produto> tmp = await App.Db.GetAll();

        tmp.ForEach(i => lista.Add(i));
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

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        DisplayAlert("Total", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
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
}