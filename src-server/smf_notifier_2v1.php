<?php
/* SMF 2.1 Notifier Query API
 * (c) 2015-2019 Benedikt Müssig <git@bmuessig.eu>
 * MIT License
*/

/* CHANGE YOUR SETTINGS BELOW */

define("BASE_SITE_URL", "http://codewalr.us");
define("SITE_INDEX_URL", BASE_SITE_URL . "/index.php");
define("SITE_TITLE", "CodeWalr.us");
define("TMP_CACHE_FILE", "smf-notify_cache_" . md5(BASE_SITE_URL) . ".db");
define("TMP_INFO_FILE", "smf-notify_info_" . md5(BASE_SITE_URL) . ".db");
define("CACHE_TIME", 40);
define("CACHE_POSTS", 50);
define("DEFAULT_MAX_POSTS", 10);
define("DEFAULT_STRIPHTML_MODE", "NONE");

/* Custom client styles */
define("SERVE_CLIENT_STYLES", true);
define("CLIENT_HEADER_BG_COLOR", "#6EB489");
define("CLIENT_TITLE_FG_COLOR", "#FFFFFF");
define("CLIENT_SUBTITLE_FG_COLOR", "#D8D8D8");
define("CLIENT_BODY_BG_COLOR", "#FAFAFA");
define("CLIENT_FOOTER_BG_COLOR", "#FAFAFA");
define("CLIENT_FOOTER_FG_COLOR", "#C6C6C6");
define("CLIENT_TITLE_FONT", "Tahoma 16");
define("CLIENT_DETAIL_FONT", "Tahoma 11");
define("CLIENT_BODY_HTML", "<html>\n<head>\n<style>\nbody {\n font-family: 'Tahoma';\n}\n\nimg {\n max-width:100%;\n height:auto;\n}\n\na {\n text-decoration:none;\n}\n</style>\n</head>\n<body>\n<post>\n</body>\n</html>");
define("CLIENT_BODY_ANTIALIAS", true);

/* DO NOT CHANGE ANYTHING BELOW THIS LINE! */

// Static internal constants
define("API_VERSION_MAJOR", 4);
define("API_VERSION_MINOR", 6);
define("API_VERSION_REV", 0);

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

			if(!$cached) {
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
			if(is_string($posts == null)) {
				ThrowException($exceptions, "PROGRAM_EXCEPTION", $posts);
				$success = false;
			}

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
} else if(isset($_GET['styles'])) {
	if(SERVE_CLIENT_STYLES) {
		$json = json_encode(array(	"success"		=> true,
									"colors"		=> array(	"header_bg" 	=> CLIENT_HEADER_BG_COLOR,
																"title_fg"		=> CLIENT_TITLE_FG_COLOR,
																"subtitle_fg"	=> CLIENT_SUBTITLE_FG_COLOR,
																"body_bg"		=> CLIENT_BODY_BG_COLOR,
																"footer_bg"		=> CLIENT_FOOTER_BG_COLOR,
																"footer_fg"		=> CLIENT_FOOTER_FG_COLOR
														),
									"text"			=> array(	"title_font"		=> CLIENT_TITLE_FONT,
																"detail_font" 		=> CLIENT_DETAIL_FONT,
																"body_html"			=> CLIENT_BODY_HTML,
																"body_anti_alias"	=> CLIENT_BODY_ANTIALIAS
														)
						));
	} else {
		$json = json_encode(array(	"success"	=> false,
									"colors"	=> null,
									"text"		=> null
							));
	}
} else {
	$json = json_encode(array(	"whoami"		=> "SMF Notifier Query API",
								"version"		=> array(API_VERSION_MAJOR, API_VERSION_MINOR, API_VERSION_REV),
								"configuration"	=> array(	"cache_ttl" 		=> CACHE_TIME,
															"cache_posts"		=> CACHE_POSTS,
															"site_url"			=> BASE_SITE_URL,
															"site_title"		=> SITE_TITLE,
															"provide_styles"	=> SERVE_CLIENT_STYLES
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
		$recentPosts = (new SimpleXMLElement($rawXml))->xpath('//item');
		
		$outputData = array();
		$postCount = 0;
		
		foreach ($recentPosts as $post) {
			if($postCount >= $maxPostCount)
				break;
			
			// Get the id from that link
			// Thank you so much, SMF2.1 for doing this to me
			// You just had to remove so much useful info...
			$postUrl = strval($post->link);
			preg_match("/^\\W*(https?:\\/\\/(?:.*)=(\\d+)).msg(\\d+)(?:#msg.*)?\\W*$/", $postUrl, $idMatches);
			if(count($idMatches) != 4)
				return "Could not parse the link IDs!";
			
			// Get the attributes
			$topicUrl = strval($idMatches[1]);
			$topicId = intval($idMatches[2]);
			$postId = intval($idMatches[3]);
			
			array_push($outputData,
						array(	"post"		=> array(	"subject"	=> StripHtmlEntities(strval($post->title)), // req'd
														"body"		=> StripHtml(strval($post->description), $htmlMode), // req'd
														"id"		=> $postId, // req'd
														"time"		=> ToUnixTime(strval($post->pubDate)), // req'd
														"link"		=> strval($post->link) // req'd
												),
								"poster"	=> array(	"name"	=> "SMF 2.1 <3", // req'd
														"id"	=> 0, // req'd
														"link"	=> ""
													),
								"topic"		=> array(	"subject"	=> StripHtmlEntities(strval($post->title)), // req'd
														"id"		=> $topicId, // req'd
														"link"		=> $topicUrl // req'd
													),
								"starter"	=> array(	"name"		=> "Unknown",
														"id"		=> 0,
														"link"		=> ""
													),
								"board"		=> array(	"name"		=> "Unknown",
														"id"		=> 0,
														"link"		=> ""
													)
							));
							
			$postCount++;
		}
		
		return $outputData;
	} catch (Exception $ex) {
		return strval($ex);
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