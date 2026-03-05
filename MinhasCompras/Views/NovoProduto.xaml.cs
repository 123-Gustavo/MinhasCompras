using MinhasCompras.Models;
using System.Threading.Tasks;

namespace MinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = double.Parse(txt_quantidade.Text),
                Preco = double.Parse(txt_preco.Text)
            };

            await App.Db.Insert(p);
            await DisplayAlert("Sucesso", "Produto cadastrado com sucesso!", "OK");

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}