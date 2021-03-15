<?php
try {
    include("./inc/connect.inc");

    if ($_SERVER['REQUEST_METHOD'] === 'GET') {
        include("./inc/head.inc");
        include("./inc/nav.inc");
        include("./inc/content/start.inc");
        inc_content("stream", "Stream");
        inc_content("tldr", "TL;DR");
        inc_content("facts", "Anleitung");
        inc_content("rules", "Disclaimer und Regeln");
        inc_content("impressum", "Impressum und Datenschutz");
        include("./inc/foot.inc");
    }
}
catch(Exception $e) {
    echo($e->getMessage());
};

