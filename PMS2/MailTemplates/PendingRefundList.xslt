<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <body>
        <table>
          <tr>
            <p>
              Pending Refunded List
            </p>
          </tr>
          <tr>
            <td colspan="1">
              <xsl:for-each select="REFUNDXsltRoot">
                <table style="width: 100%; background: black;" border="1" cellSpacing="0" cellPadding="0">
                  <tr>
                    <td style="padding-bottom: 1.5pt; padding-left: 1.5pt; padding-right: 1.5pt; background: #eff3f7; padding-top: 1.5pt;width: 60%">
                      <span style="font-family: Arial,sans-serif,FONT-SIZE: 10pt">
                        <b>JobOrderNo</b>
                      </span>
                    </td>
                    <td style="padding-bottom: 1.5pt; padding-left: 1.5pt; padding-right: 1.5pt; background: #eff3f7; padding-top: 1.5pt;width:80%">
                      <span style="font-family: Arial,sans-serif,FONT-SIZE: 10pt,">
                        <b>CustomerName</b>
                      </span>
                    </td>
                    <td style="padding-bottom: 1.5pt; padding-left: 1.5pt; padding-right: 1.5pt; background: #eff3f7; padding-top: 1.5pt;width:70%">
                      <span style="font-family: Arial,sans-serif,FONT-SIZE: 10pt,width:60%">
                        <b>AwbNumber</b>
                      </span>
                    </td>
                    <td style="padding-bottom: 1.5pt; padding-left: 1.5pt; padding-right: 1.5pt; background: #eff3f7; padding-top: 1.5pt;width:80%;">
                      <span style="font-family: Arial,sans-serif,FONT-SIZE: 10pt,width:60%">
                        <b>RefundInstrumentNumber</b>
                      </span>
                    </td>
                    <td style="padding-bottom: 1.5pt; padding-left: 1.5pt; padding-right: 1.5pt; background: #eff3f7; padding-top: 1.5pt;width:80%;">
                      <span style="font-family: Arial,sans-serif,FONT-SIZE: 10pt,width:600%">
                        <b>AmountDeposited</b>
                      </span>
                    </td>
                  </tr>
                  <xsl:for-each select="REFUND">
                    <tr>
                      <td style="padding-bottom: 1.5pt; padding-left: 1.5pt; padding-right: 1.5pt; background: #eff3f7; padding-top: 1.5pt;">
                        <span style="font-family: Arial,sans-serif,FONT-SIZE: 8pt;">
                          <xsl:value-of select="JobOrderNo" />
                        </span>
                      </td>
                      <td style="padding-bottom: 1.5pt; padding-left: 1.5pt; padding-right: 1.5pt; background: white; padding-top: 1.5pt;">
                        <span style="font-family: Arial,sans-serif,FONT-SIZE: 8pt">
                          <xsl:value-of select="CustomerName " />
                        </span>
                      </td>
                      <td style="padding-bottom: 1.5pt; padding-left: 1.5pt; padding-right: 1.5pt; background: #eff3f7; padding-top: 1.5pt;">
                        <span style="font-family: Arial,sans-serif,FONT-SIZE: 8pt">
                          <xsl:value-of select="AwbNumber" />
                        </span>
                      </td>
                      <td style="padding-bottom: 1.5pt; padding-left: 1.5pt; padding-right: 1.5pt; background: white; padding-top: 1.5pt;">
                        <span style="font-family: Arial,sans-serif,FONT-SIZE: 8pt">
                          <xsl:value-of select="RefundInstrumentNumber" />
                        </span>
                      </td>
                      <td style="padding-bottom: 1.5pt; padding-left: 1.5pt; padding-right: 1.5pt; background: #eff3f7; padding-top: 1.5pt;">
                        <span style="font-family: Arial,sans-serif,FONT-SIZE: 8pt">
                          <xsl:value-of select="AmountDeposited"/>
                        </span>
                      </td>
                    </tr>
                  </xsl:for-each>
                </table>
              </xsl:for-each>

            </td>
          </tr>
        </table>
        <!--<table>
          <tr></tr>
          <tr></tr>
          <tr></tr>
          <xsl:for-each select="_x0040_TEMPCUST">
            <tr>
              <td>
                Thanks and Regards
              </td>
            </tr>

            <tr></tr>
            <tr></tr>
            <tr></tr>
            --><!--<tr>
              <xsl:variable name="ProjUrl" select="ProjUrl" />
              <td>
                <a href="{$ProjUrl}">
                  <xsl:value-of select="ProjUrl" />
                </a>
              </td>
            </tr>--><!--
            <table  class="table" >
              <tr>
                <td>
                  <br></br>
                  <br></br>
                  <br></br>
                  <br></br>
                  <br></br>
                  <br></br>
                </td>
              </tr>
              <tr >
                <td>
                  <p>
                    <xsl:variable name="CourierSoftwarePoweredby" select="CourierSoftwarePoweredby" />
                    <xsl:value-of select="$CourierSoftwarePoweredby" />
                  </p>
                </td>
              </tr>
            </table>
          </xsl:for-each>
        </table>-->
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>