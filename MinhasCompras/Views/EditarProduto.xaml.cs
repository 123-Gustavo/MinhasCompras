namespace MinhasCompras.Views;

using MinhasCompras.Models;
using System;
using MinhasCompras.Helpers;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Categoria = txt_categoria.Text,
                Quantidade = double.Parse(txt_quantidade.Text),
                Preco = double.Parse(txt_preco.Text)
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso", "Produto atualizado com sucesso!", "OK"); 
            await Navigation.PopAsync();

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}