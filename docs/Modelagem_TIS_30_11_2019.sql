-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema mpmg
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `mpmg` ;

-- -----------------------------------------------------
-- Schema mpmg
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mpmg` DEFAULT CHARACTER SET utf8 ;
USE `mpmg` ;

-- -----------------------------------------------------
-- Table `mpmg`.`departamento`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`departamento` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`departamento` (
  `id_dpto` INT(11) NOT NULL,
  `nome_dpto` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id_dpto`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mpmg`.`municipio`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`municipio` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`municipio` (
  `id_municipio` INT(11) NOT NULL AUTO_INCREMENT,
  `nome_municipio` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id_municipio`))
ENGINE = InnoDB
AUTO_INCREMENT = 72
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mpmg`.`tabelausuario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`tabelausuario` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`tabelausuario` (
  `sgdp` INT(11) NOT NULL COMMENT 'Código identificador da tabela.\\\\n',
  `id_municipio_referente` INT(11) NOT NULL,
  `id_municipio` INT(11) NOT NULL,
  `dt_geracao` DATE NOT NULL,
  `ano_referente` VARCHAR(4) NOT NULL,
  `titulo_aba_1` VARCHAR(120) NOT NULL,
  `titulo_aba_2` VARCHAR(120) NOT NULL,
  `titulo_aba_3` VARCHAR(120) NOT NULL,
  `analista_resp` VARCHAR(90) NULL DEFAULT NULL,
  PRIMARY KEY (`sgdp`),
  INDEX `fk_TabelaUsuario_Municipio1_idx` (`id_municipio` ASC) VISIBLE,
  INDEX `fk_TabelaUsuario_Municipio2_idx` (`id_municipio_referente` ASC) VISIBLE,
  CONSTRAINT `fk_TabelaUsuario_Municipio1`
    FOREIGN KEY (`id_municipio`)
    REFERENCES `mpmg`.`municipio` (`id_municipio`),
  CONSTRAINT `fk_TabelaUsuario_Municipio2`
    FOREIGN KEY (`id_municipio_referente`)
    REFERENCES `mpmg`.`municipio` (`id_municipio`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mpmg`.`uploadtabelafam`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`uploadtabelafam` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`uploadtabelafam` (
  `id_upload` INT(11) NOT NULL,
  `dt_upload` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_upload`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mpmg`.`tabelafam`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`tabelafam` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`tabelafam` (
  `mes` CHAR(2) NOT NULL,
  `ano` CHAR(4) NOT NULL,
  `id_upload` INT(11) NOT NULL,
  `valor_fam` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`mes`, `ano`, `id_upload`),
  INDEX `fk_tabelafam1_idx` (`mes` ASC, `ano` ASC) VISIBLE,
  INDEX `fk_TabelaFAM_UploadTabelaFAM1_idx` (`id_upload` ASC) VISIBLE,
  CONSTRAINT `fk_TabelaFAM_UploadTabelaFAM1`
    FOREIGN KEY (`id_upload`)
    REFERENCES `mpmg`.`uploadtabelafam` (`id_upload`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mpmg`.`notafiscal`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`notafiscal` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`notafiscal` (
  `id_nota_fiscal` INT(11) NOT NULL AUTO_INCREMENT,
  `sgdp` INT(11) NOT NULL,
  `mes_fam` CHAR(2) NOT NULL,
  `ano_fam` CHAR(4) NOT NULL,
  `id_upload_fam` INT(11) NOT NULL,
  `id_dpto` INT(11) NULL DEFAULT NULL,
  `nr_nota_fiscal` INT(11) NOT NULL,
  `dt_emissao` DATE NOT NULL,
  `vrtotal` DECIMAL(7,3) NOT NULL,
  `preco_medio_revenda` DECIMAL(7,3) NOT NULL,
  `preco_maximo_revenda` DECIMAL(7,3) NOT NULL,
  `dt_consulta_anp` DATE NOT NULL,
  `id_tipo_combustivel` VARCHAR(3) NOT NULL,
  `quantidade` INT(11) NOT NULL,
  `preco_unitario` DECIMAL(7,2) NOT NULL,
  `nro_folha` INT(11) NULL DEFAULT NULL,
  `veiculo` VARCHAR(50) NULL DEFAULT NULL,
  `placa_veiculo` VARCHAR(7) NULL DEFAULT NULL,
  PRIMARY KEY (`id_nota_fiscal`),
  INDEX `fk_NotaFiscal_TabelaUsuario1_idx` (`sgdp` ASC) VISIBLE,
  INDEX `fk_NotaFiscal_Departamento1_idx` (`id_dpto` ASC) VISIBLE,
  INDEX `fk_notafiscal_tabelafam1_idx` (`mes_fam` ASC, `ano_fam` ASC, `id_upload_fam` ASC) VISIBLE,
  CONSTRAINT `fk_NotaFiscal_Departamento1`
    FOREIGN KEY (`id_dpto`)
    REFERENCES `mpmg`.`departamento` (`id_dpto`),
  CONSTRAINT `fk_NotaFiscal_TabelaUsuario1`
    FOREIGN KEY (`sgdp`)
    REFERENCES `mpmg`.`tabelausuario` (`sgdp`),
  CONSTRAINT `fk_notafiscal_tabelafam1`
    FOREIGN KEY (`mes_fam` , `ano_fam` , `id_upload_fam`)
    REFERENCES `mpmg`.`tabelafam` (`mes` , `ano` , `id_upload`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mpmg`.`cupomfiscal`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`cupomfiscal` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`cupomfiscal` (
  `coo` VARCHAR(45) NOT NULL,
  `sgdp` INT(11) NOT NULL,
  `id_nota_fiscal` INT(11) NULL DEFAULT NULL,
  `posto_referente` VARCHAR(90) NULL DEFAULT NULL,
  `hodometro` INT(11) NULL DEFAULT NULL,
  `cliente` VARCHAR(90) NULL DEFAULT NULL,
  `dt_emissao` DATETIME NULL DEFAULT NULL,
  `quantidade` INT(11) NULL DEFAULT NULL,
  `preco_unitario` DECIMAL(7,2) NULL DEFAULT NULL,
  `vr_total` DECIMAL(7,2) NULL DEFAULT NULL,
  `id_tipo_combustivel` CHAR(3) NULL DEFAULT NULL,
  PRIMARY KEY (`coo`),
  INDEX `fk_CupomFiscal_NotaFiscal1_idx` (`id_nota_fiscal` ASC) VISIBLE,
  INDEX `fk_CupomFiscal_TabelaUsuario1_idx` (`sgdp` ASC) VISIBLE,
  CONSTRAINT `fk_CupomFiscal_NotaFiscal1`
    FOREIGN KEY (`id_nota_fiscal`)
    REFERENCES `mpmg`.`notafiscal` (`id_nota_fiscal`),
  CONSTRAINT `fk_CupomFiscal_TabelaUsuario1`
    FOREIGN KEY (`sgdp`)
    REFERENCES `mpmg`.`tabelausuario` (`sgdp`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mpmg`.`municipioreferente`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`municipioreferente` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`municipioreferente` (
  `id_municipio` INT(11) NOT NULL,
  `id_municipio_referente` INT(11) NOT NULL,
  `ano` CHAR(4) NOT NULL,
  PRIMARY KEY (`id_municipio`, `id_municipio_referente`, `ano`),
  INDEX `fk_municipio_referente` (`id_municipio_referente` ASC) VISIBLE,
  CONSTRAINT `fk_municipio`
    FOREIGN KEY (`id_municipio`)
    REFERENCES `mpmg`.`municipio` (`id_municipio`),
  CONSTRAINT `fk_municipio_referente`
    FOREIGN KEY (`id_municipio_referente`)
    REFERENCES `mpmg`.`municipio` (`id_municipio`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mpmg`.`uploadanp`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`uploadanp` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`uploadanp` (
  `id_upload_anp` INT(11) NOT NULL AUTO_INCREMENT,
  `data` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_upload_anp`))
ENGINE = InnoDB
AUTO_INCREMENT = 15
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mpmg`.`tabelaanp`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`tabelaanp` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`tabelaanp` (
  `id_municipio` INT(11) NOT NULL AUTO_INCREMENT,
  `mes` CHAR(2) NOT NULL,
  `ano` CHAR(4) NOT NULL,
  `produto` VARCHAR(45) NOT NULL,
  `id_upload_anp` INT(11) NULL DEFAULT NULL,
  `preco_medio_revenda` DECIMAL(7,3) NOT NULL,
  `preco_maximo_revenda` DECIMAL(7,3) NOT NULL,
  `preco_minimo_revenda` DECIMAL(7,3) NOT NULL,
  `dt_vigencia_municipio_anp` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`id_municipio`, `mes`, `ano`, `produto`),
  INDEX `fk_TabelaANP_Municipio1_idx` (`id_municipio` ASC) VISIBLE,
  INDEX `fk_TabelaANP_UploadANP1_idx` (`id_upload_anp` ASC) VISIBLE,
  CONSTRAINT `fk_TabelaANP_Municipio1`
    FOREIGN KEY (`id_municipio`)
    REFERENCES `mpmg`.`municipio` (`id_municipio`),
  CONSTRAINT `fk_TabelaANP_UploadANP1`
    FOREIGN KEY (`id_upload_anp`)
    REFERENCES `mpmg`.`uploadanp` (`id_upload_anp`))
ENGINE = InnoDB
AUTO_INCREMENT = 268
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;


-- MOCK DEPARTAMENTO
INSERT INTO `departamento`(`id_dpto`, `nome_dpto`) VALUES (0, 'Teste');

-- MUNICIPIOS INICIAIS
INSERT INTO `Municipio` 
(`nome_municipio`)
VALUES
("ALFENAS"),
("ARAGUARI"),
("ARAXA"),
("BARBACENA"),
("BELO HORIZONTE"),
("BETIM"),
("BOM DESPACHO"),
("CAMPO BELO"),
("CARATINGA"),
("CATAGUASES"),
("CONGONHAS"),
("CONSELHEIRO LAFAIETE"),
("CONTAGEM"),
("CORONEL FABRICIANO"),
("CURVELO"),
("DIAMANTINA"),
("DIVINOPOLIS"),
("FORMIGA"),
("FRUTAL"),
("GOVERNADOR VALADARES"),
("GUAXUPE"),
("IBIRITE"),
("IPATINGA"),
("ITABIRA"),
("ITAJUBA"),
("ITAUNA"),
("ITUIUTABA"),
("JANAUBA"),
("JANUARIA"),
("JOAO MONLEVADE"),
("JOAO PINHEIRO"),
("JUIZ DE FORA"),
("LAVRAS"),
("LEOPOLDINA"),
("MANHUACU"),
("MARIANA"),
("MONTE CARMELO"),
("MONTES CLAROS"),
("MURIAE"),
("NOVA LIMA"),
("OLIVEIRA"),
("OURO PRETO"),
("PARA DE MINAS"),
("PARACATU"),
("PASSOS"),
("PATOS DE MINAS"),
("PATROCINIO"),
("POCOS DE CALDAS"),
("POUSO ALEGRE"),
("RIBEIRAO DAS NEVES"),
("SABARA"),
("SANTA LUZIA"),
("SANTOS DUMONT"),
("SAO JOAO DEL REI"),
("SAO LOURENCO"),
("SAO SEBASTIAO DO PARAISO"),
("SETE LAGOAS"),
("TEOFILO OTONI"),
("TIMOTEO"),
("TRES CORACOES"),
("UBA"),
("UBERABA"),
("UBERLANDIA"),
("UNAI"),
("VARGINHA"),
("VESPASIANO"),
("VICOSA");


-- MOCK TABELA FAM
INSERT INTO uploadtabelafam VALUES (1, CURRENT_DATE);

INSERT INTO tabelafam (ano, mes, valor_fam, id_upload)
VALUES
(2013,1,1.4416323,1),
(2013,2,1.4284899,1),
(2013,3,1.4211003,1),
(2013,4,1.4126243,1),
(2013,5,1.4043387,1),
(2013,6,1.3994409,1),
(2013,7,1.3955335,1),
(2013,8,1.3973502,1),
(2013,9,1.3951175,1),
(2013,10,1.3913610,1),
(2013,11,1.3829250,1),
(2013,12,1.3754972,1),
(2014,1,1.3656645,1),
(2014,2,1.3571148,1),
(2014,3,1.3484845,1),
(2014,4,1.3375168,1),
(2014,5,1.3271651,1),
(2014,6,1.3192496,1),
(2014,7,1.3158283,1),
(2014,8,1.3141201,1),
(2014,9,1.3117589,1),
(2014,10,1.3053627,1),
(2014,11,1.3004205,1),
(2014,12,1.2935653,1),
(2015,1,1.2855944,1),
(2015,2,1.2668454,1),
(2015,3,1.2523183,1),
(2015,4,1.2336894,1),
(2015,5,1.2249920,1),
(2015,6,1.2129836,1),
(2015,7,1.2037150,1),
(2015,8,1.1967737,1),
(2015,9,1.1937892,1),
(2015,10,1.1877318,1),
(2015,11,1.1786560,1),
(2015,12,1.1657167,1),
(2016,1,1.1553189,1),
(2016,2,1.1381330,1),
(2016,3,1.1274224,1),
(2016,4,1.1224835,1),
(2016,5,1.1153453,1),
(2016,6,1.1045210,1),
(2016,7,1.0993541,1),
(2016,8,1.0923630,1),
(2016,9,1.0889868,1),
(2016,10,1.0881165,1),
(2016,11,1.0862697,1),
(2016,12,1.0855100,1),
(2017,1,1.0839925,1),
(2017,2,1.0794588,1),
(2017,3,1.0768740,1),
(2017,4,1.0734393,1),
(2017,5,1.0725810,1),
(2017,6,1.0687337,1),
(2017,7,1.0719493,1),
(2017,8,1.0701306,1),
(2017,9,1.0704513,1),
(2017,10,1.0706658,1),
(2017,11,1.0667186,1),
(2017,12,1.0648019,1),
(2018,1,1.0620406,1),
(2018,2,1.0596037,1),
(2018,3,1.0576997,1),
(2018,4,1.0569598,1),
(2018,5,1.0547449,1),
(2018,6,1.0502291,1),
(2018,7,1.0354223,1),
(2018,8,1.0328403,1),
(2018,9,1.0328403,1),
(2018,10,1.0297511,1),
(2018,11,1.0256485,1),
(2018,12,1.0282191,1),
(2019,1,1.0267814,1),
(2019,2,1.0230984,1),
(2019,3,1.0176034,1),
(2019,4,1.0098277,1),
(2019,5,1.0038049,1),
(2019,6,1.0023014,1),
(2019,7,1.0022012,1),
(2019,8,1.0012000,1),
(2019,9,1.0000000,1),
(2019,10,1.0000000,1),
(2019,11,1.0000000,1),
(2019,12,1.0000000,1);