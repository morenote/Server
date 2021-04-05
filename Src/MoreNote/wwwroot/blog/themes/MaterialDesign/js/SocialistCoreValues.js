// 社会主义核心价值观
var click_count=0;
onload = function() {
var click_cnt = 0;
var $html = document.getElementsByTagName("html")[0];
var $body = document.getElementsByTagName("body")[0];
$html.onclick = function(e) {
        click_count=click_count+10;
        var $elem = document.createElement("b");
        $elem.style.color = "#E94F06";
        $elem.style.zIndex = 9999;
        $elem.style.position = "absolute";
        $elem.style.select = "none";
        var x = e.pageX;
        var y = e.pageY;
        $elem.style.left = (x - 10) + "px";
        $elem.style.top = (y - 20) + "px";
        clearInterval(anim);
        switch (click_count) {
            case 10:
                $elem.innerText = "富强";
                break;
            case 20:
                $elem.innerText = "民主";
                break;
            case 30:
                $elem.innerText = "文明";
                break;
            case 40:
                $elem.innerText = "和谐";
                break;
            case 50:
                $elem.innerText = "自由";
                break;
            case 60:
                $elem.innerText = "平等";
                break;
            case 70:
                $elem.innerText = "公正";
                break;
            case 80:
                $elem.innerText = "法制";
                break;
            case 90:
                $elem.innerText = "爱国";
                break;
            case 100:
                $elem.innerText ="敬业";
                break;
            case 110:
               $elem.innerText = "诚信";
                break;
            case 120:
                   $elem.innerText = "友善";
                break;
            default:
                $elem.innerText = "❤";
                click_count=0;
                break;
        }
        $elem.style.fontSize = Math.random() * 10 + 8 + "px";
        var increase = 0;
        var anim;
        setTimeout(function() {
            anim = setInterval(function() {
                if (++increase == 300) {
                    clearInterval(anim);
                    $body.removeChild($elem);
                }
                $elem.style.top = y - 20 - increase + "px";
                $elem.style.opacity = (150 - increase/2) / 120;
            }, 16);
        }, 70);
        $body.appendChild($elem);
    };
};
//记住主题皮肤

$(document).ready(function () {
	var ThemeMode=localStorage.getItem("ThemeMode");//获取名称为“key”的值
	if(!ThemeMode){
		ThemeMode='dark';
		Theme.setTheme('dark');
		$("#dark-theme-switch").prop("checked",true);
		$(".form-control-label").text("Dark");
		
	}else{
		if(ThemeMode=="dark")
		{
			Theme.setTheme('dark');
			$("#dark-theme-switch").prop("checked",true);
			$(".form-control-label").text("Dark");
		}
		else{
			Theme.setTheme('light');
			$("#dark-theme-switch").prop("checked",false);
			$(".form-control-label").text("Light");
			
		}
	}
	// $("#cb1").attr("checked","checked");
  $("#dark-theme-switch").change(function() { 
		var check=	$("#dark-theme-switch").prop('checked');
		if(check){
			$(".form-control-label").text("Dark");
			localStorage.setItem("ThemeMode","dark");//获取名称为“key”的值
		}else{
			$(".form-control-label").text("Light");
			localStorage.setItem("ThemeMode","light");//获取名称为“key”的值
		}
	});
}); 
// Theme.setTheme(this.checked ? 'dark' : 'light')