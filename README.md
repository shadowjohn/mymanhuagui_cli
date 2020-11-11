# mymanhuagui_cli
<h2>cli 模式抓 manhuagui 漫畫 方法</h2>
<h3>Major Author : wzdlc1996</h3>
<h3>Author : 羽山 https://3wa.tw/</h3>
<h3>Version : 2020-11-11</h3>
<br>
<img src="snapshot/m1.png">
直接下語法抓 manhuagui 漫畫列表
<br>
<h3>使用方法：</h3>
cd dist<br>
get_manhuagui.exe<br>
get_manhuagui.exe test<br>
get_manhuagui.exe "https://www.manhuagui.com/comic/7580/523083.html"<br>

<h3>回應：</h3>
{metadata json}<br>
####################################<br>
Page 1 Link<br>
Page 2 Link<br>
Page 3 Link<br>
Page 4 Link<br>
Page 5 Link<br>
...<br>
<br>
<h3>編譯環境：</h3>
python36 (x86 32bit 版本)
<br>
<h3>相依套件：</h3>
<ul>
  <li>dukpy：需自行 C:\python36-32\scripts\pip.exe install dukpy</li>
  <li>lzstring：需自行 C:\python36-32\scripts\pip.exe install lzstring</li>
  <li>bs4：需自行 C:\python36-32\scripts\pip.exe install bs4</li>
  <li>lxml：需自行 C:\python36-32\scripts\pip.exe install lxml</li>
  <li>urllib</li>
  <li>json</li>
</ul>
<br>
<h3>下載範例：</h3>
wget "https://i.hamreus.com/ps3/y/yiquanchaoren/%E7%AC%AC179%E8%AF%9D/0TIG5n.png.webp?e=1605310131&m=vzvYBcVxGKB-CsJzX5-XDQ" --user-agent="User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36" --referer "https://www.manhuagui.com/"  -O ok.png
<h3>參考資料：</h3>
<ul>
  <li>1. https://github.com/wzdlc1996/mangaSpider</li>
  <li>2. https://wzdlc1996.github.io/artic/other/mangaspider/</li>
</ul>  
<h3>Todo：</h3>
<ul>
  <li>(2020-11-11) 1. 修正下載網址的簡體中文改成 urlencode 後的內容</li>
</ul>