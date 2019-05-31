
function generateWordMap(gdpData){
    var max = 0,
        min = Number.MAX_VALUE,
        cc,
        startColor = [25, 19, 28],
        endColor = [75, 20, 107],
        colors = { },
        hex;

    //find maximum and minimum values
    for (cc in gdpData)
    {
        if (parseFloat(gdpData[cc]) > max)
        {
            max = parseFloat(gdpData[cc]);
        }
        if (parseFloat(gdpData[cc]) < min)
        {
            min = parseFloat(gdpData[cc]);
        }
    }

    //set colors according to values of GDP
    for (cc in gdpData)
    {
        if (gdpData[cc] > 0)
        {
            colors[cc] = '#';
            for (var i = 0; i<3; i++)
            {
                hex = Math.round(startColor[i]
                    + (endColor[i]
                        - startColor[i])
                    * (gdpData[cc] / (max - min))).toString(16);

                if (hex.length == 1)
                {
                    hex = '0'+hex;
                }
                out = (hex.length == 1 ? '0' : '') + hex;
                //    colors[cc] += out.replace("-","");
                colors[cc] += out;
            }
        }
    }
    //initialize JQVMap
    jQuery('#vmap').vectorMap(
        {
            colors: colors,
            hoverOpacity: 0.7,
            hoverColor: false,
            backgroundColor: "transparent",
            onLabelShow: function (event, label, code) {
                if(gdpData[code] == null){
                    gdpData[code] = 0;
                }
                label.append("<br>"+gdpData[code]+' Total');
            },
        });
}


function timeDifference(current, previous) {

    var now = new Date(current*1000);
    var previous = new Date(previous*1000),

        secondsPast = (now.getTime() - previous.getTime()) / 1000;
    if(secondsPast < 60){
        return parseInt(secondsPast) + ' Secounds Ago';
    }
    if(secondsPast < 3600){
        return parseInt(secondsPast/60) + ' Minutes Ago';
    }
    if(secondsPast <= 86400){
        return parseInt(secondsPast/3600) + ' Hours Ago';
    }
    if(secondsPast > 86400){
        day = previous.getDate();
        month = previous.toDateString().match(/ [a-zA-Z]*/)[0].replace(" ","");
        year = previous.getFullYear() == now.getFullYear() ? "" :  " "+previous.getFullYear();
        return day + " " + month + year;
    }
}


