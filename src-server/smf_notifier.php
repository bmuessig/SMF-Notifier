<?php
/* SMF Notifier Query API
 * (c) 2015 Muessigb <muessigb@yahoo.de> (Muessigb.net)
 * MIT License
*/


/* CHANGE YOUR SETTINGS BELOW */

define("BASE_SITE_URL", "http://codewalr.us");
define("SITE_INDEX_URL", BASE_SITE_URL . "/index.php");
define("TMP_CACHE_FILE", "smf-notify_cache_" . md5(BASE_SITE_URL) . ".db");
define("TMP_INFO_FILE", "smf-notify_info_" . md5(BASE_SITE_URL) . ".db");
define("CACHE_TIME", 40);
define("CACHE_POSTS", 50);
define("DEFAULT_MAX_POSTS", 10);
define("DEFAULT_STRIPHTML_MODE", "NONE");


/* DO NOT CHANGE ANYTHING BELOW THIS LINE! */

// Static internal constants
define("API_VERSION_MAJOR", 4);
define("API_VERSION_MINOR", 3);
define("API_VERSION_REV", ord('c'));

// Internal Constants
define("STRIPHTML_NONE", 0);
define("STRIPHTML_ALL", 1);

ob_start();

date_default_timezone_set("UTC");

	if(isset($_GET['query'])) {
		$exceptions = array();
		$success = true;

		try {
			$cached = false;
			$cache = "";
			if(file_exists(TMP_CACHE_FILE)) {
				try {
					$cacheRawObj = file_get_contents(TMP_CACHE_FILE);
					if(isJson($cacheRawObj)) {
						$cacheObj = json_decode($cacheRawObj);
						if( isset($cacheObj->timestamp) && isset($cacheObj->data) ) {
							if( time() < (intval($cacheObj->timestamp) + CACHE_TIME) ) {
								$cache = strval($cacheObj->data);
								$cached = true;
							}
						}
					}
				} catch (Exception $ex) {
					$cacheJson = false;
				}
			}

			if(! $cached) {
				$queryUrl = SITE_INDEX_URL . "?action=.xml";
				$queryOpts = array("sa=recent", "limit=" . CACHE_POSTS);
				$rawInput = QueryFeed($queryUrl, $queryOpts);
				file_put_contents(TMP_CACHE_FILE, json_encode(array("timestamp" => time(), "data" => $rawInput)));
			} else {
				$rawInput = $cache;
			}

			// Check if it the information has changed (e.g. edits, new posts)
			if(file_exists(TMP_INFO_FILE)) {
				$rawInfoInput = file_get_contents(TMP_INFO_FILE);
				$infoObj = json_decode($rawInfoInput);
				$changed = intval($infoObj->changed);
				$lastHash = strval($infoObj->hash);
			} else {
				$latestVersion = false;
			}

			if(!$cached) {
				$currentHash = md5($rawInput);
				if(file_exists(TMP_INFO_FILE)) { // file does exist and is loaded
					if(strcmp(md5($rawInput), $lastHash) == 0) // hash does match
						$latestVersion = true;
					else
						$latestVersion = false;
				} else
					$latestVersion = false;
			} else
				$latestVersion = true;

			if(! $latestVersion) {
				$changed = time();
				file_put_contents(TMP_INFO_FILE, json_encode(array("changed" => $changed, "hash" => $currentHash)));
			}

			$mxposts = (isset($_GET['max_posts']) ? ParseInt($_GET['max_posts'], DEFAULT_MAX_POSTS) : DEFAULT_MAX_POSTS);

			if(isset($_GET['html_stripmode'])) {
				$htmllvl = ParseStripmode($_GET['html_stripmode'], false);
				if($htmllvl === false) {
					$htmllvl = ParseStripmode(DEFAULT_STRIPHTML_MODE);
					ThrowException($exceptions, "INVALID_ARGUMENT", "html_stripmode");
				}
			} else {
				$htmllvl = ParseStripmode(DEFAULT_STRIPHTML_MODE);
			}

			if($mxposts > CACHE_POSTS)
				$mxposts = DEFAULT_MAX_POSTS;
				
			$posts = UltraSMFParser($rawInput, $mxposts, $htmllvl);

	} catch (Exception $ex) {
		ThrowException($exceptions, "PROGRAM_EXCEPTION", print_r($ex, true));
		$success = false;
	}

	if($success) {
		$json = json_encode(array(		"success"		=> true,
										"exceptions"	=> $exceptions,
										"cached"		=> $cached,
										"timestamp"		=> time(),
										"changed"		=> $changed,
										"data"			=> $posts,
								));
	} else {
		$json = json_encode(array(		"success"		=> false,
										"exceptions"	=> $exceptions,
										"cached"		=> null,
										"timestamp"		=> time(),
										"changed"		=> null,
										"data"			=> null,
								));
	}
} else {
	$json = json_encode(array(	"whoami"		=> "SMF Notifier Query API",
								"version"		=> array(API_VERSION_MAJOR, API_VERSION_MINOR, API_VERSION_REV),
								"configuration"	=> array(	"cache_ttl" 	=> CACHE_TIME,
															"cache_posts"	=> CACHE_POSTS,
															"site_url"		=> BASE_SITE_URL
														),
								"defaults"		=> array(	"max_posts"			=> DEFAULT_MAX_POSTS,
															"html_stripmode" 	=> DEFAULT_STRIPHTML_MODE
														)
						));
}

header('Content-Type: application/json');
print(json_minify($json));
print_gzipped_output();

function UltraSMFParser($rawXml, $maxPostCount, $htmlMode)
{
	try {
		$recentPosts = (new SimpleXMLElement($rawXml));
		$outputData = array();
		$postCount = 0;
		
		foreach ($recentPosts->{'recent-post'} as $post) {
			if($postCount >= $maxPostCount)
				break;
		
			array_push($outputData,
						array(	"post"		=> array(	"subject"	=> StripHtmlEntities(strval($post->subject)),
														"body"		=> StripHtml(strval($post->body), $htmlMode),
														"id"		=> intval($post->id),
														"time"		=> ToUnixTime(strval($post->time)),
														"link"		=> strval($post->link)
												),
								"poster"	=> array(	"name"	=> StripHtmlEntities(strval($post->poster->name)),
														"id"	=> intval($post->poster->id),
														"link"	=> strval($post->poster->link)
													),
								"topic"		=> array(	"subject"	=> StripHtmlEntities(strval($post->topic->subject)),
														"id"		=> intval($post->topic->id),
														"link"		=> strval($post->topic->link)
													),
								"starter"	=> array(	"name"		=> StripHtmlEntities(strval($post->starter->name)),
														"id"		=> intval($post->starter->id),
														"link"		=> strval($post->starter->link)
													),
								"board"		=> array(	"name"		=> StripHtmlEntities(strval($post->board->name)),
														"id"		=> intval($post->board->id),
														"link"		=> strval($post->board->link)
													)
							));
							
			$postCount++;
		}
		
		return $outputData;
	} catch (Exception $ex) {
		echo $ex;
	}
}

function ToUnixTime($smfStr)
{
	// Convert to unix time
	return strtotime(str_replace(" at ", ", ", $smfStr));
}

function StripHtmlEntities($str)
{
	return html_entity_decode($str, (ENT_QUOTES | ENT_HTML401));
}

function StripHtml($str, $mode)
{
	$outStr = $str;
	
	// If needed do something with the most common tags
	if($mode > STRIPHTML_NONE) {
		// Handle line breaks
		$outStr = preg_replace("/<br \/>/", "\n", $outStr);
		// Handle smileys
		$outStr = preg_replace("/<img src=\"Smileys\/[^\n\"\=]*\" alt=\"([^\n\"\=]*)\" title=\"[^\n\"\=]*\" class=\"smiley\" \/>/", "$1", $outStr);
	}
	
	// Finalize the output if allowed
	if($mode == STRIPHTML_ALL) {
		// Delete all tags
		$outStr = preg_replace("/<[^<>]*>/", "", $outStr);
		// Replace html entitys
		$outStr = StripHtmlEntities($outStr);
	}
	
	if($mode == STRIPHTML_NONE) {
		// Fix smilies
		$outStr = preg_replace("/<img src=\"(Smileys[^\n\r\"]*?)\" ([^<>\n\r]*?)>/", "<img src=\"". BASE_SITE_URL . "/$1\" $2>", $outStr);
	}
	
	return $outStr;
}

function QueryFeed($baseUrl, $opts)
{
	// Concernate the different pieces together, glue them with ;
	$query = implode(";", $opts);
	// Send request to the server and retrieve answer
	return file_get_contents($baseUrl . (strlen($query)>0 ? (";" . $query) : ""));
}

function ParseStripmode($str, $def="none")
{
	$stripmodes = array("none", "all");
	$stripmodeindex = 0;
	foreach ($stripmodes as $stripmode) {
		if (stripos($str, $stripmode) !== false) {
			return $stripmodeindex;
		}
		$stripmodeindex++;
	}
	return (is_string($def) ? ParseStripmode($def) : $def);
}

function ParseInt($str, $def=false)
{
	return (is_numeric($str) ? intval($str) : $def);
}

function ThrowException(&$stack, $type, $what="", $critical=false)
{
	array_push($stack, array(	"type"		=> $type,
								"what"		=> $what,
								"critical"	=> $critical
				));
}

function print_gzipped_output() 
{ 
	if(! isset($_SERVER["HTTP_ACCEPT_ENCODING"]) )
		$encoding = false;
	else {
		$HTTP_ACCEPT_ENCODING = $_SERVER["HTTP_ACCEPT_ENCODING"]; 
		if( headers_sent() ) 
			$encoding = false; 
		else if( strpos($HTTP_ACCEPT_ENCODING, 'x-gzip') !== false ) 
			$encoding = 'x-gzip'; 
		else if( strpos($HTTP_ACCEPT_ENCODING,'gzip') !== false ) 
			$encoding = 'gzip'; 
		else 
			$encoding = false;
	}
    
    if( $encoding ) 
    { 
        $contents = ob_get_clean(); 
        $_temp1 = strlen($contents); 
        if ($_temp1 < 2048)    // no need to waste resources in compressing very little data 
            print($contents); 
        else 
        { 
            header('Content-Encoding: '.$encoding); 
            print("\x1f\x8b\x08\x00\x00\x00\x00\x00"); 
            $contents = gzcompress($contents, 9); 
            $contents = substr($contents, 0, $_temp1); 
            print($contents); 
        } 
    } 
    else 
        ob_end_flush(); 
}

/*! JSON.minify()
	v0.1 (c) Kyle Simpson
	MIT License
*/
function json_minify($json) {
	$tokenizer = "/\"|(\/\*)|(\*\/)|(\/\/)|\n|\r/";
	$in_string = false;
	$in_multiline_comment = false;
	$in_singleline_comment = false;
	$tmp; $tmp2; $new_str = array(); $ns = 0; $from = 0; $lc; $rc; $lastIndex = 0;
		
	while (preg_match($tokenizer,$json,$tmp,PREG_OFFSET_CAPTURE,$lastIndex)) {
		$tmp = $tmp[0];
		$lastIndex = $tmp[1] + strlen($tmp[0]);
		$lc = substr($json,0,$lastIndex - strlen($tmp[0]));
		$rc = substr($json,$lastIndex);
		if (!$in_multiline_comment && !$in_singleline_comment) {
			$tmp2 = substr($lc,$from);
			if (!$in_string) {
				$tmp2 = preg_replace("/(\n|\r|\s)*/","",$tmp2);
			}
			$new_str[] = $tmp2;
		}
		$from = $lastIndex;
			
		if ($tmp[0] == "\"" && !$in_multiline_comment && !$in_singleline_comment) {
			preg_match("/(\\\\)*$/",$lc,$tmp2);
			if (!$in_string || !$tmp2 || (strlen($tmp2[0]) % 2) == 0) {	// start of string with ", or unescaped " character found to end string
				$in_string = !$in_string;
			}
			$from--; // include " character in next catch
			$rc = substr($json,$from);
		}
		else if ($tmp[0] == "/*" && !$in_string && !$in_multiline_comment && !$in_singleline_comment) {
			$in_multiline_comment = true;
		}
		else if ($tmp[0] == "*/" && !$in_string && $in_multiline_comment && !$in_singleline_comment) {
			$in_multiline_comment = false;
		}
		else if ($tmp[0] == "//" && !$in_string && !$in_multiline_comment && !$in_singleline_comment) {
			$in_singleline_comment = true;
		}
		else if (($tmp[0] == "\n" || $tmp[0] == "\r") && !$in_string && !$in_multiline_comment && $in_singleline_comment) {
			$in_singleline_comment = false;
		}
		else if (!$in_multiline_comment && !$in_singleline_comment && !(preg_match("/\n|\r|\s/",$tmp[0]))) {
			$new_str[] = $tmp[0];
		}
	}
	$new_str[] = $rc;
	return implode("",$new_str);
}

function isJson($string) {
	json_decode($string);
	return (json_last_error() == JSON_ERROR_NONE);
}

?>