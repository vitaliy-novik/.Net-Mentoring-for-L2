<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:ext="http://epam.com/xsl/ext"
				xmlns:lc="http://library.by/catalog"
				xml:space="default">
	<xsl:output method="xml" indent="yes" />

	<xsl:template match="/">
		<rss version="2.0">
			<xsl:apply-templates/>
		</rss>
	</xsl:template>
	
	<xsl:template match="lc:book">
		<item>
			<xsl:apply-templates />
			<xsl:if test="lc:genre='Computer' and lc:isbn">
				<link>
					<xsl:value-of select="ext:FormatUri(string(lc:isbn))"/>
				</link>
			</xsl:if>
		</item>
	</xsl:template>
	
	<xsl:template match="/lc:catalog">
		<channel>
			<title>Rss feed</title>
			<link>Not sure were it should point</link>
			<description>Some description</description>
			<xsl:apply-templates />
		</channel>
	</xsl:template>

	<xsl:template match="lc:title">
		<title>
			<xsl:value-of select="."/>
		</title>
	</xsl:template>

	<xsl:template match="lc:description">
		<description>
			<xsl:value-of select="."/>
		</description>
	</xsl:template>

	<xsl:template match="lc:registration_date">
		<pubDate>
			<xsl:value-of select="."/>
		</pubDate>
	</xsl:template>

	<xsl:template match="*" />
	<xsl:template match="text()" />
	
</xsl:stylesheet>
