namespace MyEverything.ThisMvc.Models;

public class AllDatas
{ 
    List<Data_DTO> datas;
    public AllDatas()
    {
        datas = new List<Data_DTO>
        {
            new Data_DTO
            {

                Title = ".Net Notlar",
                Image = "https://www.yusufsezer.com.tr/dosyalar/2019/03/net.png",
                Explanation = ".net ile ilgili çeşitli notlar. Bu notlarla çığır aşacaksınız. Ben aslında yokmuşum diyeceksiniz :D..."
                
            },
            new Data_DTO
            {
                Title = ".Net 10 da ne var",
                Image = "https://startdebugging.net/wp-content/uploads/2024/12/net10-hero-min.png",
                Explanation = "Sürekli gelişen yazılım dünyasında .Net  10 da neler var"
            },
            new Data_DTO
            {
                Title = "Arduino ile akıllı çöp kutusu",
                Image = "https://i.ytimg.com/vi/rTA_bdzG_W4/hqdefault.jpg",
                Explanation = "Çöp o kadar akıllı ki insanın kıskanası geliyor"
            },
            new Data_DTO
            {
                Title = "Piston Aşağı",
                Image = "https://arabam-blog.mncdn.com/wp-content/uploads/2020/12/piston-kapak-gorsel.jpg",
                Explanation = "Yorum yok..."
            }
        };

        Datas.AddDatas(datas);

    }
}