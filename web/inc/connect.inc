<?php


$light = false;
function getBGClass()
{
    global $light;
    if ($light)
    {
        $light = false;
        return "bg-light";
    }
    else
    {
        $light = true;
        return "";
    }
}

function inc_content($name, $caption)
{
    ?>
    <div class="<?=getBGClass()?> <?=$name?>-container section-container">
        <div class="container <?=$name?>">
            <div class="row">
                <div class="col <?=$name?> section-description wow fadeIn">
                    <h2><?=$caption?></h2>
                    <div class="divider-1 wow fadeInUp"><span></span></div>
                </div>
            </div>
            <div class="row">
                <?php include("./inc/content/".$name.".inc"); ?>
            </div>
        </div>
    </div>
    <?php
}

?>
