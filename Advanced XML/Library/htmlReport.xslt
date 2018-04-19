<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:lc="http://library.by/catalog"
				xml:space="default">
	<xsl:output method="xml" indent="yes" />
	
	<xsl:param name="Date" select="''" />

	<xsl:key name="genre" use="." match="//lc:genre"/>

	<xsl:template match="/">
		<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<meta charset="utf-8" />
				<title>Current funds by genre</title>
			</head>
			<body>
				<h1>
					<xsl:value-of select="$Date" />: Current funds by genre
				</h1>
				<xsl:apply-templates />
				<xsl:call-template name="totalBooks" />
			</body>
		</html>
	</xsl:template>

	<xsl:template match="/lc:catalog">
		<xsl:for-each select="//lc:genre[count(. | key('genre', .)[1]) = 1]">
			<h2>
				<xsl:value-of select="."/>
			</h2>
			<table>
				<xsl:call-template name="tableHeader" />
				<xsl:call-template name="booksByGenre">
					<xsl:with-param name="genre">
						<xsl:value-of select="."/>
					</xsl:with-param>
				</xsl:call-template>
			</table>
			<xsl:call-template name="totalBooks">
				<xsl:with-param name="genre">
					<xsl:value-of select="."/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="booksByGenre">
		<xsl:param name="genre"/>
		<xsl:for-each select="//lc:book[lc:genre = $genre]">
			<tr>
				<xsl:call-template name="tableCell">
					<xsl:with-param name="data">
						<xsl:value-of select="lc:author"/>
					</xsl:with-param>
				</xsl:call-template>
				<xsl:call-template name="tableCell">
					<xsl:with-param name="data">
						<xsl:value-of select="lc:title"/>
					</xsl:with-param>
				</xsl:call-template>
				<xsl:call-template name="tableCell">
					<xsl:with-param name="data">
						<xsl:value-of select="lc:publish_date"/>
					</xsl:with-param>
				</xsl:call-template>
				<xsl:call-template name="tableCell">
					<xsl:with-param name="data">
						<xsl:value-of select="lc:registration_date"/>
					</xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="tableCell">
		<xsl:param name="data" />
		<td>
			<xsl:value-of select="$data"/>
		</td>
	</xsl:template>

	<xsl:template name="tableHeader">
		<tr>
			<th>Author</th>
			<th>Title</th>
			<th>Publish date</th>
			<th>Registration date</th>
		</tr>
	</xsl:template>

	<xsl:template name="totalBooks">
		<xsl:param name="genre" />
		<xsl:choose>
			<xsl:when test="not($genre)">
				<hr/>
				<b>
					Total: <xsl:value-of select="count(//lc:book)"/>
				</b>
			</xsl:when>
			<xsl:otherwise>
				<b>
					Total: <xsl:value-of select="count(//lc:book[lc:genre = $genre])"/>
				</b>
			</xsl:otherwise>
		</xsl:choose>
		
	</xsl:template>

	<xsl:template match="*" />
	<xsl:template match="text()" />

</xsl:stylesheet>