public class Pagination
{
    public int Maxpagesize {get;set;} = 1000;
    public int _pagesize {get;set;} = 100;
    public int pagenumber {get;set;}
    public int Pagesize { get => _pagesize; set => _pagesize = (value > Maxpagesize) ? Maxpagesize : value; }
}