using System.Collections.Generic;

public class myStruct
{
    public int bid { get; set; }
    public string bname { get; set; }
    public string bpic { get; set; }
    public int cid { get; set; }
    public string cname { get; set; }
    public List<string> files { get; set; }
    public string finished { get; set; }
    public int len { get; set; }
    public string path { get; set; }
    public string path_encode { get; set; }
    public int status { get; set; }
    public string block_cc { get; set; }
    public int nextId { get; set; }
    public int prevId { get; set; }
    public sl sl { get; set; }
}
public class sl
{
    public int e { get; set; }

    public string m { get; set; }

}