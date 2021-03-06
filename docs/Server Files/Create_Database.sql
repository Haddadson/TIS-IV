-- MySQL Script generated by MySQL Workbench
-- Sun Dec 15 05:24:52 2019
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';


-- -----------------------------------------------------
-- Table `municipio`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `municipio` (
  `id_municipio` INT(11) NOT NULL AUTO_INCREMENT,
  `nome_municipio` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id_municipio`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `tabelausuario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabelausuario` (
  `sgdp` VARCHAR(20) NOT NULL COMMENT 'Código identificador da tabela.',
  `id_municipio` INT(11) NOT NULL,
  `id_municipio_referente` INT(11) NOT NULL,
  `dt_geracao` DATE NOT NULL,
  `titulo_aba_1` VARCHAR(120) NOT NULL,
  `titulo_aba_2` VARCHAR(120) NOT NULL,
  `titulo_aba_3` VARCHAR(120) NOT NULL,
  `analista_resp` VARCHAR(90) NULL DEFAULT NULL,
  PRIMARY KEY (`sgdp`),
  INDEX `fk_tabelausuario_municipio1_idx` (`id_municipio` ASC),
  INDEX `fk_tabelausuario_municipio2_idx` (`id_municipio_referente` ASC),
  CONSTRAINT `fk_tabelausuario_municipio1`
    FOREIGN KEY (`id_municipio`)
    REFERENCES `municipio` (`id_municipio`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tabelausuario_municipio2`
    FOREIGN KEY (`id_municipio_referente`)
    REFERENCES `municipio` (`id_municipio`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `anosportabela`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `anosportabela` (
  `sgdp` VARCHAR(20) NOT NULL,
  `anoreferente` CHAR(4) NOT NULL,
  PRIMARY KEY (`sgdp`, `anoreferente`),
  INDEX `fk_tabelausuario_has_anoreferente_tabelausuario1_idx` (`sgdp` ASC),
  CONSTRAINT `fk_tabelausuario_has_anoreferente_tabelausuario1`
    FOREIGN KEY (`sgdp`)
    REFERENCES `tabelausuario` (`sgdp`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `cupomfiscal`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `cupomfiscal` (
  `sgdp` VARCHAR(20) NOT NULL,
  `coo` VARCHAR(20) NOT NULL,
  `nr_nota_fiscal` VARCHAR(15) NULL DEFAULT NULL,
  `posto_referente` VARCHAR(90) NULL DEFAULT NULL,
  `hodometro` INT(11) NULL DEFAULT NULL,
  `cliente` VARCHAR(90) NULL DEFAULT NULL,
  `dt_emissao` DATETIME NULL DEFAULT NULL,
  `quantidade` DECIMAL(7,3) NULL DEFAULT NULL,
  `preco_unitario` DECIMAL(7,3) NULL DEFAULT NULL,
  `vrtotal` DECIMAL(7,3) NULL DEFAULT NULL,
  `produto` VARCHAR(45) NULL DEFAULT NULL,
  `veiculo` VARCHAR(50) NULL DEFAULT NULL,
  `placa_veiculo` VARCHAR(15) NULL DEFAULT NULL,
  PRIMARY KEY (`sgdp`, `coo`),
  INDEX `fk_cupomfiscal_tabelausuario1_idx` (`sgdp` ASC),
  CONSTRAINT `fk_cupomfiscal_tabelausuario1`
    FOREIGN KEY (`sgdp`)
    REFERENCES `tabelausuario` (`sgdp`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `departamento`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `departamento` (
  `id_dpto` INT(11) NOT NULL,
  `nome_dpto` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id_dpto`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `uploadtabelafam`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `uploadtabelafam` (
  `id_upload` INT(11) NOT NULL,
  `dt_upload` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_upload`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `tabelafam`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabelafam` (
  `id_upload` INT(11) NOT NULL,
  `mes` CHAR(2) NOT NULL,
  `ano` CHAR(4) NOT NULL,
  `valor_fam` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id_upload`, `mes`, `ano`),
  INDEX `fk_tabelafam1_idx` (`mes` ASC, `ano` ASC),
  INDEX `fk_TabelaFAM_UploadTabelaFAM1_idx` (`id_upload` ASC),
  CONSTRAINT `fk_TabelaFAM_UploadTabelaFAM1`
    FOREIGN KEY (`id_upload`)
    REFERENCES `uploadtabelafam` (`id_upload`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `notafiscal`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `notafiscal` (
  `sgdp` VARCHAR(20) NOT NULL,
  `nr_nota_fiscal` VARCHAR(15) NOT NULL,
  `id_upload_fam` INT(11) NOT NULL REFERENCES `tabelafam` (`id_upload`),
  `mes_fam` CHAR(2) NOT NULL REFERENCES `tabelafam` (`mes_fam`),
  `ano_fam` CHAR(4) NOT NULL REFERENCES `tabelafam` (`ano_fam`),
  `id_dpto` INT(11) NULL DEFAULT NULL,
  `dt_emissao` DATE NOT NULL,
  `vrtotal_nota` DECIMAL(7,3) NOT NULL,
  `preco_medio_revenda` DECIMAL(7,3) NOT NULL,
  `preco_maximo_revenda` DECIMAL(7,3) NOT NULL,
  `dt_consulta_anp` DATE NOT NULL,
  `nro_folha` INT(11) NULL DEFAULT NULL,
  `veiculo` VARCHAR(50) NULL DEFAULT NULL,
  `placa_veiculo` VARCHAR(20) NULL DEFAULT NULL,
  PRIMARY KEY (`sgdp`, `nr_nota_fiscal`),
  INDEX `fk_NotaFiscal_TabelaUsuario1_idx` (`sgdp` ASC),
  INDEX `fk_NotaFiscal_Departamento1_idx` (`id_dpto` ASC),
  INDEX `fk_notafiscal_tabelafam1_idx` (`mes_fam` ASC, `ano_fam` ASC, `id_upload_fam` ASC),
  CONSTRAINT ``
    FOREIGN KEY (`id_dpto`)
    REFERENCES `departamento` (`id_dpto`),
  CONSTRAINT `fk_NotaFiscal_TabelaUsuario1`
    FOREIGN KEY (`sgdp`)
    REFERENCES `tabelausuario` (`sgdp`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `itemnotafiscal`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `itemnotafiscal` (
  `sgdp` VARCHAR(20) NOT NULL,
  `nr_nota_fiscal` VARCHAR(15) NOT NULL,
  `id_item_nota_fiscal` INT(11) NOT NULL,
  `produto` VARCHAR(45) NOT NULL,
  `quantidade` DECIMAL(7,3) NOT NULL,
  `vunitario` DECIMAL(7,3) NOT NULL,
  `vrtotal` DECIMAL(7,3) NOT NULL,
  PRIMARY KEY (`sgdp`, `nr_nota_fiscal`, `id_item_nota_fiscal`),
  INDEX `fk_itemnotafiscal_notafiscal1_idx` (`sgdp` ASC, `nr_nota_fiscal` ASC),
  CONSTRAINT `fk_itemnotafiscal_notafiscal1`
    FOREIGN KEY (`sgdp` , `nr_nota_fiscal`)
    REFERENCES `notafiscal` (`sgdp` , `nr_nota_fiscal`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `municipioreferente`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `municipioreferente` (
  `sgdp` VARCHAR(20) NOT NULL,
  `id_municipio` INT(11) NOT NULL,
  `id_municipio_referente` INT(11) NOT NULL,
  `anoreferente` VARCHAR(45) NOT NULL,
  `mesreferente` CHAR(2) NOT NULL,
  PRIMARY KEY (`sgdp`, `id_municipio`, `id_municipio_referente`, `anoreferente`, `mesreferente`),
  INDEX `fk_municipio_referente` (`id_municipio_referente` ASC),
  INDEX `fk_municipioreferente_municipio1_idx` (`id_municipio` ASC),
  INDEX `fk_municipioreferente_tabelausuario1_idx` (`sgdp` ASC),
  CONSTRAINT `fk_municipio_referente`
    FOREIGN KEY (`id_municipio_referente`)
    REFERENCES `municipio` (`id_municipio`),
  CONSTRAINT `fk_municipioreferente_municipio1`
    FOREIGN KEY (`id_municipio`)
    REFERENCES `municipio` (`id_municipio`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_municipioreferente_tabelausuario1`
    FOREIGN KEY (`sgdp`)
    REFERENCES `tabelausuario` (`sgdp`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `uploadanp`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `uploadanp` (
  `id_upload_anp` INT(11) NOT NULL AUTO_INCREMENT,
  `data` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_upload_anp`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `tabelaanp`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabelaanp` (
  `id_municipio` INT(11) NOT NULL AUTO_INCREMENT,
  `mes` CHAR(2) NOT NULL,
  `ano` CHAR(4) NOT NULL,
  `produto` VARCHAR(45) NOT NULL,
  `id_upload_anp` INT(11) NULL DEFAULT NULL,
  `preco_medio_revenda` DECIMAL(7,3) NOT NULL,
  `preco_maximo_revenda` DECIMAL(7,3) NOT NULL,
  `dt_vigencia_municipio_anp` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`id_municipio`, `mes`, `ano`, `produto`),
  INDEX `fk_TabelaANP_Municipio1_idx` (`id_municipio` ASC),
  INDEX `fk_TabelaANP_UploadANP1_idx` (`id_upload_anp` ASC),
  CONSTRAINT `fk_TabelaANP_Municipio1`
    FOREIGN KEY (`id_municipio`)
    REFERENCES `municipio` (`id_municipio`),
  CONSTRAINT `fk_TabelaANP_UploadANP1`
    FOREIGN KEY (`id_upload_anp`)
    REFERENCES `uploadanp` (`id_upload_anp`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8mb4;

ALTER TABLE notafiscal MODIFY COLUMN vrtotal_nota decimal(18,3);
ALTER TABLE itemnotafiscal MODIFY COLUMN vrtotal decimal(18,3);
ALTER TABLE cupomfiscal MODIFY COLUMN vrtotal decimal(18,3);

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
