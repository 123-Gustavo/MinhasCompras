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
            lst_produtos.IsRefreshing = true;

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
        }finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        var grupos = lista
        .GroupBy(p => p.Categoria)
        .Select(g => new
        {
            Categoria = g.Key,
            Total = g.Sum(p => p.Total)
        });

        string msg = "";

        foreach (var item in grupos)
        {
            msg += $"{item.Categoria}: {item.Total:C}\n";
        }

        DisplayAlert("Total por Categoria", msg, "OK");
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

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        base.OnAppearing();
        try
        {

            await CarregarProdutos();

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void txt_search_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        try
        {

            string q = e.NewTextValue;
            lst_produtos.IsRefreshing = true;

            if (string.IsNullOrWhiteSpace(q))
            {
                await CarregarProdutos();
                return;
            }

            lista.Clear();

            List<Produto> tmp = await App.Db.SearchCategoria(q);

            tmp.ForEach(i => lista.Add(i));

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
}